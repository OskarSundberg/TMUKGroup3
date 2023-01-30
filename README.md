# Chat application

[![Build Status for Chat program][build-status]][build-log]

---

# GitHub handles

* simondereuver (Simon de Reuver)

* Pramell94 (Gustav Pråmell)

* samwalle (Samuel Wallander Leyongberg)

* OskarSundberg (Oskar Sundberg)

* ivonilsson (Ivo Nilsson)

--- 

# The Plan

We are going to make a chat-service hosted either locally or remotely. It is going to be a chatroom
service with several features such as a general chatroom for clients connected to the server and direct 
messaging from client to client over the server. All messages are going to be encrypted which makes our 
chat-service secure. We are going to create a GUI in WPF which is connected to the code-behind client 
files. 

## Language

We will use C#

## Build System

We will use .NET in CLI as the build system

## Compiling and running instructions

First make sure you have the [.NET SDK](https://dotnet.microsoft.com/en-us/download) installed  

To run Server open `...\TMUKGroup3` as current directory in CLI

Input:

``` 
dotnet build .\Server\Server.sln
dotnet run --project .\Server\Server\Server.csproj
```

Now the server is started! Time to start a client

Open `...\TMUKGroup3` as current directory in CLI

Input:

``` 
dotnet build .\Client\Client.sln
dotnet run --project .\Client\ClientPresentation\ClientPresentation.csproj
```

Now you have a working Client.

---

## Unit test

For Server unit tests input: 
    dotnet test ./Server/Server.sln
For Client uni tests input:
	dotnet test ./Client/Client.sln

---

## Code Coverage

First make sure to install .NET tool

``` 
dotnet tool install --global dotnet-coverage
dotnet tool install -g dotnet-reportgenerator-globaltool 
```

Open `...\TMUKGroup3\Server` or `...\TMUKGroup3\Client` as current directory in CLI

Now run this command to generate a .cobertura report

    dotnet coverage collect dotnet test --output .\coveragereport\Codecovarge --output-format cobertura

Now to convert the cobertura files to HTML run it's command

    reportgenerator -reports:".\coveragereport\Codecovarge.cobertura.xml" -targetdir:"coveragereport" -reporttype:Html

You will now find a index.htm files in the coveragereport folder.

---

## Linter

Too use a linter in the project use the command

    dotnet format

for more specific scenarois use

`dotnet format whitespace` For fixing whitespace

`dotnet format style` Runs code style analyzer

for more commands check out [Github](https://github.com/dotnet/format)

---

# Kanban

This is a link to the [Kanban](https://github.com/users/OskarSundberg/projects/2/views/1)

---

I, Simon de Reuver, declare that I am the sole author of the content I add to this repository.

I, Gustav Pråmell, declare that I am the sole author of the content I add to this repository.

I, Samuel Wallander Leyongberg, declare that I am the sole author of the content I add to this repository.

I, Oskar Sundberg, declare that I am the sole author of the content I add to this repository.

I, Ivo Nilsson, declare that I am the sole author of the content I add to this repository.

---

[build-log]:    https://github.com/OskarSundberg/TMUKGroup3/actions/workflows/build.yml
[build-status]: https://github.com/OskarSundberg/TMUKGroup3/actions/workflows/build.yml/badge.svg