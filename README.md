## <p align=center> Projeto Blog refatorado para uma API em ASP.NET 6 </p>

Este projeto visa ser uma aplicação real a partir do curso de [Fundamentos do Entity Framework](https://github.com/lanzath/entity-framework-fundamentals)


## Configuração de serviços por Injeção de Dependência
Há 3 modos para configurar o uso de serviços na aplicação, são estas:

**Transient**

A cada requisição de serviço por injeção de dependência, sempre é gerada uma nova instância.

```cs
builder.Services.AddTransient<CustomService>();
```

**Scoped**

Gera uma instância do serviço por transação, ou seja, verifica se já houve uma requisição daquele serviço na transação em questão para que seja reaproveitada a instância.

```cs
builder.Services.AddScoped<CustomService>();
```

**Singleton**

É criada apenas **uma única instância** do serviço por aplicação.

```cs
builder.Services.AddSingleton<CustomService>();
```

## Estrutura do projeto

##### Controllers

Responsáveis por definir as ações de cada `Endpoint`.

##### Data

Configuração do `DataContext` e **mapeamento** de `models` com definições sobre as tabelas e suas respectivas colunas para a geração correta das `migrations`

##### Extensions

São métodos de extensão que adicionamos a outras classes, são sempre estáticos e devemos no parâmetro informar a qual classe queremos adicionálos com o `this`

##### Migrations

Arquivos responsável por gerar, atualizar e versionar o banco de dados.

##### Models

São as entidades que representam o banco de dados, possuem propriedades e métodos de regra de negócio.

##### Services

Configuração de serviços utilizados pela aplicação e regras de negócio.

##### View Models

São responsáveis por representar apenas o conteúdo necessário dos dados que vem pela requisição.

##### wwwroot
É para onde os arquivos estáticos são enviados, como uploads de imagens.

Para aceitar arquivos estáticos deve ser adicionado ao `program.cs`
`app.UseStaticFiles();`.

As imagens foram upadas aqui para fins didáticos, não é uma boa prática armazenar arquivos na API, deve ser utilizado um serviço de storage como por exemplo o **Azure Storage** ou **AWS S3**
