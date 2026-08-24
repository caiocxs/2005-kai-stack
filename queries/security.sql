SELECT 
    s.session_id,
    s.login_name,
    s.host_name,
    s.program_name,
    s.client_interface_name,
    c.client_net_address AS ip_origem,
    c.client_tcp_port AS porta_origem,
    s.status,
    s.login_time,
    s.last_request_start_time
FROM sys.dm_exec_sessions s
INNER JOIN sys.dm_exec_connections c 
    ON s.session_id = c.session_id
WHERE s.is_user_process = 1;

-- Busca no log de erro todas as tentativas que falharam (inclui IP e motivo)
EXEC xp_readerrorlog 0, 1, N'Login failed';

-- Ajusta o tempo limite de consultas remotas / conexões para 60 segundos
EXEC sp_configure 'remote query timeout', 60;
RECONFIGURE;