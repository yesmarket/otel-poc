# Otel-Module.psm1
Write-Host "Loading Otel-Module"

function Clean-Docker {
   <#
   .SYNOPSIS
      Helper function to clean docker containers, volumes, networks, and images.
   .DESCRIPTION
      Helper function to clean docker containers, volumes, networks, and images.
   .PARAMETER containers
      Specifies, whether or not, docker containers will be removed.
   .PARAMETER volumes
      Specifies, whether or not, docker volumes will be removed.
   .PARAMETER networks
      Specifies, whether or not, docker networks will be removed.
   .PARAMETER networks
      Specifies, whether or not, docker images will be removed.
   .Example
      Clean-Docker -containers $true -volumes $true -networks $true
   #>
   Param(
      [bool]$containers=$true,
      [bool]$volumes=$true,
      [bool]$networks=$true,
      [bool]$images=$false
   )
   if (!(Get-Command docker))
   {
      echo 'docker required'
      return
   }
   if ($containers) {
      if (docker ps -q) {
         echo "stopping running containers..."
         docker stop $(docker ps -q) > $null
      }
      if (docker ps -a -q) {
         echo "removing containers..."
         docker rm $(docker ps -a -q) > $null
      }
   }
   if ($volumes -And (docker volume ls -q)) {
      echo "removing volumes..."
      docker volume rm $(docker volume ls -q) > $null
   }
   if ($networks -And (docker network ls -f "type=custom" -q)) {
      echo "removing networks..."
      docker network rm $(docker network ls -f "type=custom" -q) > $null
   }
   if ($images -And (docker image ls -a -q)) {
      echo "removing images..."
      docker rmi $(docker image ls -a -q) > $null
   }
}

function Clean-Docker2 {
   Param(
      [string]$images='otel'
   )
   Clean-Docker -containers $true -volumes $true -networks $true -images $false
   if ($images -And (docker image ls -a -q)) {
      $arr = $images.Split(",")
      $arr | ForEach-Object { iex "docker rmi -f otel-poc_$_" }
   }
}

$otel_module = $MyInvocation.MyCommand.ScriptBlock.Module
$otel_module.OnRemove = {Write-Host "Removed Otel-Module"}
