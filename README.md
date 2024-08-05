# TarefasAPI

Este repositório é a API do aplicativo [Tarefas](https://github.com/altair-noberto/Tarefas).

# Instalação

**Requisitos:**

[.NET 8](https://dotnet.microsoft.com/pt-br/download/dotnet/8.0)
[SQL Server 2022](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads)
[(Opcional) Visual Studio 2022](https://visualstudio.microsoft.com/pt-br/downloads/)

## Configuração do CORS e banco de dados

Após clonar o repositório, abra o arquivo ".\TarefasAPI\\appsettings.json", e altere o valor da terceira linha *chave "Connection:"* para a string de conexão com seu banco de dados SQL Server, como por exemplo:

```
"ConnectionStrings": {
  "Connection": "Server=LOCALHOST\\SQLEXPRESS;Database=TarefasDB;Trusted_Connection=true;TrustServerCertificate=True;"
},
```

Em seguida, vá até o arquivo "TarefasAPI\\Settings.cs", e altere a variável CORS para o endereço de IP onde o frontend está hospedado, por exemplo:

```
public static string Cors = "http://localhost:5500";
```

Em seguida, abra o terminal do "Gerenciador de pacotes Nuget".

No visual studio 2022: Na barra superior, vá para "Ferramentas -> Gerenciador de pacotes do Nuget -> Console do gerenciador de pacotes".

No terminal, é necessário instalar o [CLI do NuGet](https://learn.microsoft.com/pt-br/nuget/consume-packages/install-use-packages-nuget-cli).

No CLI, execute os seguintes comandos: 
```
'cd .\TarefasAPI\'
```
(Entra na pasta do projeto)

```
dotnet tool install --global dotnet-ef
```
Instala o entity framework tools no terminal.

```
dotnet ef database update
```
Gera o banco de dados.

Após isso, basta inicializar o projeto no Visual Studio 2022, ou executar o seguinte comando na pasta do projeto:
```
dotnet watch
```

## (Opcional) Chave da credencial para os tokens JWT

No arquivo "Settings.cs" há uma string de uma chave utilizada para criação da credencial do token JWT, caso queira alterar, insira uma chave que ao ser codificada retorna uma credencial de 512 bits, recomendo criar uma chave de pelo menos 65 caracteres com letras, números e símbolos.

## Bibliotecas adicionais

[Entity Framework Core (+Tools, Design, SqlServer)](https://learn.microsoft.com/pt-br/ef/core/) - Para integração com o banco de dados SQL Server.
[Bcrypt.Net-Next](https://github.com/BcryptNet/bcrypt.net) - Para criptografia das senhas de autenticação.
[Swashbuckle.AspNetCore] - Adiciona recursos para teste de autorização com o Bearer no Swagger
[IdentityModel.Tokens] - Adiciona recursos para implementação do Token JWT.
