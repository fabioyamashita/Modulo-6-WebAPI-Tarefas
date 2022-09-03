# Módulo 6 - Programação Web III | Web API - Tarefas

## Completar WebAPI Requests (SteamAPI_Exercise - até 01/09/2022)

Implementar os métodos que faltaram na aula:
- Delete
- Patch
- Post

## Completar WebAPI (SteamAPI_Filters - até 03/09/2022)

- Exercitar a criação/utilização dos filtros
- Tentar criar cenários de uma chamada não dê certo (short-circuit)
- Aplicar na API se possível o controle e tratamento de exceções

## Completar WebAPI com Logging (SteamAPI_Logs - até 05/09/2022)

Faça um filter, e crie uma classe específica para gravar logs, que escreva no console sempre que os endpoints de alteração (put, patch) ou remoção (delete) forem usados, indicando o horário formatado como o datetime a seguir: 01/01/2021 13:45:00.

A linha de log deve ter o seguinte formato (se a requisição for válida):

<datetime> - Game <id> - <titulo> - <Remover|Alterar (e descrever a alteração)>

Exemplo:

01/01/2021 13:45:00 - Game 1 - Counter Strike - Removido
01/01/2021 13:45:00 - Game 1 - Counter Strike - Alterado de X para Y
