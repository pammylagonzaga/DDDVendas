# 🛒 Sistema de Gestão de Vendas - DDD & Clean Architecture

Este projeto é uma aplicação de vendas desenvolvida seguindo os princípios do **Domain-Driven Design (DDD)** e **Clean Architecture**, baseada nos estudos realizados através do canal do Macoratti.

## 🏗️ Arquitetura do Projeto

O projeto está dividido em camadas para garantir o desacoplamento e a testabilidade:

* **Domain**: O coração da aplicação. Contém as Entidades e Regras de Negócio.
* **Application**: Camada de orquestração. Contém os Commands, Handlers e DTOs para execução dos Casos de Uso.
* **Infrastructure**: Implementações concretas de acesso a dados, integração com banco de dados e serviços externos.
* **API**: Camada de entrada com os Controllers e configuração do Program.cs.
* **Blazor**: Interface de usuário desenvolvida em Blazor.

## 🚀 Tecnologias e Performance

* **.NET 8+**
* **C#** (utilizando recursos modernos como `records`, `init-only properties`)
* **Entity Framework Core**
* **Domain Notification & Guard Clauses** (para validações de domínio)

## 🛠️ Padrões de Projeto Aplicados

### 1. Domain-Driven Design (DDD)
* **Entidades**: Classes com identidade única e ciclo de vida.
* **Value Objects**: Objetos imutáveis que definem atributos (ex: `NomeProduto`).
* **Agregados**: O `Produto` atua como uma raiz de agregado, protegendo sua consistência interna.
* **Domain Events**: Disparados após ações bem-sucedidas no domínio (ex: `EstoqueAjustadoEvent`).

### 2. CQRS & Imutabilidade
* Uso de **Commands** e **Handlers** selados (modificador  `sealed` ) para reforçar que estas classes possuem responsabilidade única e não devem ser estendidas.
* Uso de propriedades `init` em DTOs para garantir que os dados não sejam alterados após a criação.
* Uso de `nameof` para garantir refatoração segura em cláusulas de guarda.

 ** Agradecimentos ao Prof. Macoratti pelo excelente conteúdo sobre arquitetura de software.**

