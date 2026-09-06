using AutoMapper;
using Backend.Application.DTOs;
using Backend.Application.Interfaces;
using Backend.Domain.Entities;
using Backend.Domain.Interfaces.Repositories;
using Backend.Domain.Services;

namespace Backend.Application.Services;

public class AuthenticationService : IAuthenticationService
{
  private readonly IRepositoryManager _repositoryManager;
  private readonly IPasswordHasher _hasher;
  private readonly IMapper _mapper;
  private readonly ITokenService _tokenService;

  public AuthenticationService(
      IRepositoryManager repositoryManager,
      IPasswordHasher hasher,
      IMapper mapper,
      ITokenService tokenService)
  {
    _repositoryManager = repositoryManager;
    _hasher = hasher;
    _mapper = mapper;
    _tokenService = tokenService;
  }

  public async Task<AuthResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
  {
    if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
      return new AuthResult(false, "Username and password are required.");

    var user = await _repositoryManager.UserRepository.GetByUsernameAsync(request.Username, cancellationToken);
    return await AuthenticateAsync(user, request.Password, "Invalid username or password.", cancellationToken);
  }

  public async Task<AuthResult> LoginAsync(LoginWithEmailRequest request, CancellationToken cancellationToken = default)
  {
    if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
      return new AuthResult(false, "Email and password are required.");

    var user = await _repositoryManager.UserRepository.GetByEmailAsync(request.Email, cancellationToken);
    return await AuthenticateAsync(user, request.Password, "Invalid email or password.", cancellationToken);
  }

  private async Task<AuthResult> AuthenticateAsync(User? user, string password, string invalidCredentialsMessage, CancellationToken cancellationToken)
  {
    if (user is null)
      return new AuthResult(false, invalidCredentialsMessage);

    try
    {
      bool isValid = user.Authenticate(password, _hasher);
      await _repositoryManager.UserRepository.UpdateAsync(user, cancellationToken);

      if (!isValid)
        return new AuthResult(false, invalidCredentialsMessage);

      var userDto = _mapper.Map<UserDto>(user);
      var (token, expiresAt) = _tokenService.GenerateToken(user);
      return new AuthResult(true, "Authentication successful.", userDto, token, expiresAt);
    }
    catch (UnauthorizedAccessException ex)
    {
      await _repositoryManager.UserRepository.UpdateAsync(user, cancellationToken);
      return new AuthResult(false, ex.Message);
    }
  }

  public async Task<AuthResult> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
  {
    var existingUser = await _repositoryManager.UserRepository.GetByUsernameAsync(request.Username, cancellationToken);
    if (existingUser is not null)
      return new AuthResult(false, "Username is already in use.");
    existingUser = await _repositoryManager.UserRepository.GetByEmailAsync(request.Email, cancellationToken);
    if (existingUser is not null)
      return new AuthResult(false, "Email is already in use.");

    User user;
    try
    {
      user = User.Create(
          request.Name,
          request.Username,
          request.Email,
          request.Password,
          request.Permissions,
          _hasher
      );
    }
    catch (ArgumentException ex)
    {
      return new AuthResult(false, ex.Message);
    }

    await _repositoryManager.UnitOfWork.BeginTransactionAsync(cancellationToken);
    try
    {
      await _repositoryManager.UserRepository.CreateAsync(user, cancellationToken);
      await _repositoryManager.UnitOfWork.CommitAsync(cancellationToken);
    }
    catch
    {
      await _repositoryManager.UnitOfWork.RollbackAsync(cancellationToken);
      throw;
    }

    var userDto = _mapper.Map<UserDto>(user);
    var (token, expiresAt) = _tokenService.GenerateToken(user);
    return new AuthResult(true, "User created successfully.", userDto, token, expiresAt);
  }
}
