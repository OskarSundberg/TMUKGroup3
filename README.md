# Chat application


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

Open ...\TMUKGroup3\Server\Server as current directory in CLI

Input 

``` dotnet build ```

``` dotnet run ```

Now the server is started! Time to start a client

Open ...\TMUKGroup3\Client\ClientPresentation as current directory in CLI

Input 

``` dotnet build ```

``` dotnet run ```

Now you have a working Client.

---

## Code Coverage

First make sure to install .NET tool

``` dotnet tool install --global dotnet-coverage ```

``` dotnet tool install -g dotnet-reportgenerator-globaltool ```

Open ```...\TMUKGroup3\Server``` or ```...\TMUKGroup3\Client``` as current directory in CLI

Now run this command to generate a .cobertura report

``` dotnet coverage collect dotnet test --output .\coveragereport\Codecovarge --output-format cobertura ```

Now to convert the cobertura files to HTML run it's command

``` reportgenerator -reports:".\coveragereport\Codecovarge.cobertura.xml" -targetdir:"coveragereport" -reporttype:Html ```

You will now find a index.htm files in the coveragereport folder.

---

## Linter

Too use a linter in the project use the command

    dotnet format

for more specific scenarois use

``` dotnet format whitespace ``` For fixing whitespace

``` dotnet format style ``` Runs code style analyzer

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