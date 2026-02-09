[CmdletBinding()]
Param(
    [Parameter(Position=0,Mandatory=$false,ValueFromRemainingArguments=$true)]
    [string[]]$BuildArguments
)
dotnet run --project .\Bacon.Run\Bacon.Run.csproj -- $BuildArguments