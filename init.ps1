$module_path = $env:PSModulePath.split(';')[0] + "/Otel-Module"
if (Get-Module -Name package-scripts) {
   echo "removing existing powerShell module..."
   Remove-Module package-scripts
   rmdir $module_path -Force -Recurse
}
echo "creating powerShell module..."
New-Item -ItemType Directory -Force -Path $module_path > $null
cp ./package-scripts.psm1 $module_path/Otel-Module.psm1
New-ModuleManifest -Path $module_path/Otel-Module.psd1 -RootModule Otel-Module.psm1 -Author 'Ryan Bartsch' -CompanyName 'FlexiGroup' -Description 'A few helper functions to run containerised microservice applications on local machine' -PowerShellVersion '5.0'
echo "importing module for current session..."
Import-Module ./package-scripts -Global -DisableNameChecking
