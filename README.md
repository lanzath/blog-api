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