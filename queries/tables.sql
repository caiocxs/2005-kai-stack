USE kanannon;
GO


CREATE TABLE role (
	id INT PRIMARY KEY NOT NULL,
	type NVARCHAR(255) NOT NULL
);
GO

-- 2. Tabela de Usuários (Users)
CREATE TABLE [user] (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    name NVARCHAR(150) NOT NULL,
    username NVARCHAR(50) NOT NULL,
    email NVARCHAR(255) NOT NULL,
    password_hash NVARCHAR(255) NOT NULL,
    permissions INT NOT NULL DEFAULT 0,
    
    -- Proteção contra ataques de força bruta
    access_failed_count INT NOT NULL DEFAULT 0,
    is_locked BIT NOT NULL DEFAULT 0,
    lockout_end DATETIMEOFFSET NULL,
    
    -- Auditoria básica de domínio
    created_at DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),
    updated_at DATETIMEOFFSET NULL,
	desactivated_at DATETIMEOFFSET NULL, 
    
    -- Constraints de unicidade
    CONSTRAINT uq_users_username UNIQUE (username),
    CONSTRAINT uq_users_email UNIQUE (email),
    
    CONSTRAINT fk_user_type
        FOREIGN KEY (permissions) REFERENCES [role](id) ON DELETE CASCADE,
);
GO

-- 3. Tabela de Tokens (Refresh Tokens)
CREATE TABLE refresh_token (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    id_user UNIQUEIDENTIFIER NOT NULL,
    token_hash VARCHAR(255) NOT NULL,
    
    expires_at DATETIMEOFFSET NOT NULL,
    created_at DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),
    created_by_ip VARCHAR(45) NULL,
    revoked_at DATETIMEOFFSET NULL,
    revoked_by_ip VARCHAR(45) NULL,
    replaced_by VARCHAR(255) NULL,

    CONSTRAINT fk_refresh_tokens_user 
        FOREIGN KEY (id_user) REFERENCES [user](id) ON DELETE CASCADE,
    CONSTRAINT uq_refresh_token_hash UNIQUE (token_hash)
);
GO

-- Índices de performance para autenticação frequente
CREATE NONCLUSTERED INDEX idx_refresh_token_user_id ON refresh_token(id_user);
CREATE NONCLUSTERED INDEX idx_refresh_token_lookup ON refresh_token(token_hash) 
    INCLUDE (user_id, expires_at, revoked_at);
GO

CREATE TABLE history_type (
	id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	type NVARCHAR(256) NOT NULL
);
GO

CREATE TABLE history_user (
	id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	id_user UNIQUEIDENTIFIER NOT NULL,
	id_type UNIQUEIDENTIFIER NOT NULL,
	
	updated_at DATETIMEOFFSET NULL,
	updated_by_ip VARCHAR(64) NULL,
	
	CONSTRAINT fk_history_user_user
		FOREIGN KEY (id_user) REFERENCES [user](id) ON DELETE CASCADE,
	CONSTRAINT fk_history_user_type
		FOREIGN KEY (id_type) REFERENCES history_type(id) ON DELETE CASCADE
);
GO

CREATE TABLE page_access_metric (
	id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	id_user UNIQUEIDENTIFIER NULL, -- NULL caso o visitante seja anônimo
	
	page_route NVARCHAR(200) NOT NULL, -- Ex: '/dashboard', '/profile'
	http_method VARCHAR(10) NOT NULL, -- GET, POST
	status_code INT NOT NULL, -- 200, 404, 500
	duration_ms INT NOT NULL, -- Tempo de resposta em milissegundos
	
	ip_address VARCHAR(45) NULL,
	user_agent NVARCHAR(500) NULL,
	referrer_url NVARCHAR(500) NULL,
	accessed_at DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),
	
	CONSTRAINT fk_page_metric_user 
	  FOREIGN KEY (id_user) REFERENCES [user](id) ON DELETE SET NULL
);
GO

CREATE NONCLUSTERED INDEX idx_page_access_metric_route_date 
    ON page_access_metric(page_route, accessed_at DESC);

CREATE NONCLUSTERED INDEX idx_page_access_metric_user 
    ON page_access_metric(id_user, accessed_at DESC) 
    WHERE id_user IS NOT NULL;
GO

CREATE TABLE user_page (
	id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	id_user UNIQUEIDENTIFIER NOT NULL,
	slug NVARCHAR(100) NOT NULL,
	title NVARCHAR(150) NOT NULL,
	is_public BIT NOT NULL DEFAULT 0,
	created_at DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),
	updated_at DATETIMEOFFSET NULL,
	
	CONSTRAINT fk_user_page_user 
	    FOREIGN KEY (id_user) REFERENCES [user](id) ON DELETE CASCADE,
	CONSTRAINT uq_user_page_user_slug UNIQUE (id_user, slug)
);
GO

CREATE TABLE user_asset (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    id_user UNIQUEIDENTIFIER NOT NULL,
    file_name NVARCHAR(255) NOT NULL,
    storage_url NVARCHAR(1000) NOT NULL, -- Caminho no S3/Blob Storage/Disco
    content_type VARCHAR(100) NOT NULL,  -- Ex: 'image/png', 'image/webp'
    file_size_bytes BIGINT NOT NULL,
    created_at DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),

    CONSTRAINT fk_user_asset_user 
        FOREIGN KEY (id_user) REFERENCES [user](id) ON DELETE CASCADE
);
GO

CREATE TABLE display_type (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	display VARCHAR(20) NOT NULL
);
GO

CREATE TABLE flex_direction_type (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	flex_direction VARCHAR(20) NOT NULL
);
GO

CREATE TABLE justify_content_type (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	justify_content VARCHAR(30) NOT NULL
);
GO

CREATE TABLE align_items_type (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	align_items VARCHAR(30) NOT NULL
);
GO

CREATE TABLE flex_wrap_type (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	flex_wrap VARCHAR(20) NOT NULL
);
GO

CREATE TABLE align_self_type (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	align_self VARCHAR(20) NOT NULL
);
GO

CREATE TABLE page_element (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    id_page UNIQUEIDENTIFIER NOT NULL,
    id_parent_element UNIQUEIDENTIFIER NULL,
    id_asset UNIQUEIDENTIFIER NULL, -- NULL se for texto/HTML sem arquivo
    
    element_type VARCHAR(50) NOT NULL, -- 'image', 'text', 'container'
    
    -- Propriedades de Flexbox (Container)
    id_display UNIQUEIDENTIFIER NOT NULL, -- 'flex', 'grid', 'block'
    id_flex_direction UNIQUEIDENTIFIER NOT NULL, -- 'row', 'column', 'row-reverse', 'column-reverse'
    id_justify_content UNIQUEIDENTIFIER NOT NULL, -- 'flex-start', 'center', 'space-between', etc.
    id_align_items UNIQUEIDENTIFIER NOT NULL, -- 'stretch', 'center', 'flex-start', etc.
    gap_px INT NOT NULL DEFAULT 0,
    id_flex_wrap UNIQUEIDENTIFIER NOT NULL, -- 'nowrap', 'wrap'

    -- Propriedades de Flexbox (Item)
    flex_grow FLOAT NOT NULL DEFAULT 0.0,
    flex_shrink FLOAT NOT NULL DEFAULT 1.0,
    flex_basis VARCHAR(20) NOT NULL DEFAULT 'auto', -- 'auto', '100%', '200px'
    id_align_self UNIQUEIDENTIFIER NOT NULL,

    -- Dimensões e Estilização
    width_css VARCHAR(50) NULL,  -- '100%', '300px', 'auto'
    height_css VARCHAR(50) NULL, -- 'auto', '50vh', '150px'
    custom_text NVARCHAR(MAX) NULL,

    created_at DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),

    CONSTRAINT fk_page_element_page 
        FOREIGN KEY (id_page) REFERENCES user_page(id) ON DELETE CASCADE,
    CONSTRAINT fk_page_element_parent 
        FOREIGN KEY (id_parent_element) REFERENCES page_element(id) ON DELETE NO ACTION,
    CONSTRAINT fk_page_element_display 
        FOREIGN KEY (id_display) REFERENCES display_type(id) ON DELETE NO ACTION,
    CONSTRAINT fk_page_element_flex_direction
        FOREIGN KEY (id_flex_direction) REFERENCES flex_direction_type(id) ON DELETE NO ACTION,
    CONSTRAINT fk_page_element_justify_content 
        FOREIGN KEY (id_justify_content) REFERENCES justify_content_type(id) ON DELETE NO ACTION,
    CONSTRAINT fk_page_element_align_items 
        FOREIGN KEY (id_align_items) REFERENCES align_items_type(id) ON DELETE NO ACTION,
	CONSTRAINT fk_page_element_flex_wrap 
        FOREIGN KEY (id_flex_wrap) REFERENCES flex_wrap_type(id) ON DELETE NO ACTION,
	CONSTRAINT fk_page_element_align_self
        FOREIGN KEY (id_align_self) REFERENCES align_self_type(id) ON DELETE NO ACTION,
);
GO

CREATE NONCLUSTERED INDEX idx_user_asset_user ON user_asset(id_user);

CREATE NONCLUSTERED INDEX idx_page_element_flex_direction ON page_element(id_flex_direction);
CREATE NONCLUSTERED INDEX idx_page_element_display ON page_element(id_display);
CREATE NONCLUSTERED INDEX idx_page_element_justify_content ON page_element(id_justify_content);
CREATE NONCLUSTERED INDEX idx_page_element_align_items ON page_element(id_align_items);
CREATE NONCLUSTERED INDEX idx_page_element_flex_wrap ON page_element(id_flex_wrap);
CREATE NONCLUSTERED INDEX idx_page_element_align_self ON page_element(id_align_self);
GO

-- Tabela associativa entre Elemento (Div) e Assets
CREATE TABLE page_element_asset (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    id_page_element UNIQUEIDENTIFIER NOT NULL,
    id_asset UNIQUEIDENTIFIER NOT NULL,

    -- Ordenação dos assets dentro do mesmo elemento (ex: slide 1, slide 2)
    sort_order INT NOT NULL DEFAULT 0,

    -- Papel do asset dentro do componente (ex: 'gallery_item', 'background', 'thumbnail', 'hover_image')
    role VARCHAR(50) NOT NULL DEFAULT 'gallery_item',
    
    -- Metadados opcionais específicos do vínculo
    [caption] NVARCHAR(255) NULL,
    alt_override NVARCHAR(255) NULL,
    
    created_at DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),

    CONSTRAINT fk_pea_element 
        FOREIGN KEY (id_page_element) REFERENCES page_element(id) ON DELETE CASCADE,
    CONSTRAINT fk_pea_asset 
        FOREIGN KEY (id_asset) REFERENCES user_asset(id) ON DELETE NO ACTION,
        
    -- Impede duplicar o mesmo asset com a mesma função no mesmo bloco
    CONSTRAINT uq_pea_element_asset_role UNIQUE (id_page_element, id_asset, role)
);
GO

-- Índice cobrindo a ordem de exibição dos assets por bloco
CREATE NONCLUSTERED INDEX idx_pea_element_order 
    ON page_element_asset(id_page_element, sort_order ASC);
GO






















