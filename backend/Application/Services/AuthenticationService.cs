using AutoMapper;
using Backend.Application.DTOs;
using Backend.Application.Interfaces;
using Backend.Domain.Entities;
using Backend.Domain.Interfaces.Repositories;
using Backend.Domain.Services;

namespace Backend.Application.Services;

public class AuthenticationService : IAuthenticationService
{
  private readonly IUserRepository _userRepository;
  private readonly IPasswordHasher _hasher;
  private readonly IMapper _mapper;
  private readonly ITokenService _tokenService;

  public AuthenticationService(
      IUserRepository userRepository,
      IPasswordHasher hasher,
      IMapper mapper,
      ITokenService tokenService)
  {
    _userRepository = userRepository;
    _hasher = hasher;
    _mapper = mapper;
    _tokenService = tokenService;
  }

  public async Task<AuthResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
  {
    if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
      return new AuthResult(false, "Username and password are required.");

    var user = await _userRepository.GetByUsernameAsync(request.Username, cancellationToken);
    if (user is null)
      return new AuthResult(false, "Invalid username or password.");

    try
    {
      bool isValid = user.Authenticate(request.Password, _hasher);
      await _userRepository.UpdateAsync(user, cancellationToken);

      if (!isValid)
        return new AuthResult(false, "Invalid username or password.");

      var userDto = _mapper.Map<UserDto>(user);
      var (token, expiresAt) = _tokenService.GenerateToken(user);
      return new AuthResult(true, "Authentication successful.", userDto, token, expiresAt);
    }
    catch (UnauthorizedAccessException ex)
    {
      await _userRepository.UpdateAsync(user, cancellationToken);
      return new AuthResult(false, ex.Message);
    }
  }

  public async Task<AuthResult> LoginAsync(LoginWithEmailRequest request, CancellationToken cancellationToken = default)
  {
    if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
      return new AuthResult(false, "Email and password are required.");

    var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
    if (user is null)
      return new AuthResult(false, "Invalid email or password.");

    try
    {
      bool isValid = user.Authenticate(request.Password, _hasher);
      await _userRepository.UpdateAsync(user, cancellationToken);

      if (!isValid)
        return new AuthResult(false, "Invalid email or password.");

      var userDto = _mapper.Map<UserDto>(user);
      var (token, expiresAt) = _tokenService.GenerateToken(user);
      return new AuthResult(true, "Authentication successful.", userDto, token, expiresAt);
    }
    catch (UnauthorizedAccessException ex)
    {
      await _userRepository.UpdateAsync(user, cancellationToken);
      return new AuthResult(false, ex.Message);
    }
  }

  public async Task<AuthResult> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
  {
    var existingUser = await _userRepository.GetByUsernameAsync(request.Username, cancellationToken);
    if (existingUser is not null)
      return new AuthResult(false, "Username is already in use.");
    existingUser = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
    if (existingUser is not null)
      return new AuthResult(false, "Email is already in use.");

    try
    {
      var user = User.Create(
          request.Name,
          request.Username,
          request.Email,
          request.Password,
          request.Permissions,
          _hasher
      );

      await _userRepository.CreateAsync(user, cancellationToken);

      var userDto = _mapper.Map<UserDto>(user);
      var (token, expiresAt) = _tokenService.GenerateToken(user);
      return new AuthResult(true, "User created successfully.", userDto, token, expiresAt);
    }
    catch (ArgumentException ex)
    {
      return new AuthResult(false, ex.Message);
    }
  }
}
