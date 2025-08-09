using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.IdentityModel.Tokens;
using SistemaReservasCitas.Application.Interfaces;
using SistemaReservasCitas.Domain.Entities;

namespace SistemaReservasCitas.Authorizations;

public class JwtProvider : IJwt
{
	public readonly IConfiguration _configuration;

	public JwtProvider(IConfiguration configuration)
	{
		_configuration = configuration;
	}
	
	public string GenerarToken(Usuario usuario)
	{
		var Claims = new[]
		{
			new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
			new Claim(ClaimTypes.Name, usuario.UsuarioNombre),
			new Claim(ClaimTypes.Role, usuario.Rol.ToString())
		};

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
		var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var token = new JwtSecurityToken(
			
			issuer:  _configuration["Jwt:Issuer"],
			audience:_configuration["Jwt:Audience"],
			claims: Claims,
			expires:DateTime.Now.AddHours(2),
			signingCredentials: credentials
			
			);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}
}