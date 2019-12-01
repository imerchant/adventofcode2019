Param(
    [Parameter(Mandatory = $True)]
    [string] $projectName,
    [Parameter(Mandatory = $False)]
    [string] $lang = "C#"
)

if ($lang -ne "C#" -and $lang -ne "F#")
{
    Write-Host "Invalid language selection. Available options: [C#], [F#]"
    exit 1
}

$projExt = ".csproj"

if ($lang -eq "F#")
{
    $projExt = ".fsproj"
}

mkdir -Path "src"
mkdir -Path "test"

$testName = ($projectName + ".Tests")
$projectLocation = ("src/" + $projectName)
$testLocation = ("test/" + $testName)
$projectCsProj = ($projectName + $projExt)
$testCsProj = ($testName + $projExt)
$projectSln = ($projectName + ".sln")

# Create projects
dotnet new classlib -n $projectName -o $projectLocation -lang $lang
dotnet new xunit -n $testName -o $testLocation -lang $lang

# Add references
dotnet add $testLocation/$testCsProj package FluentAssertions
dotnet add $testLocation/$testCsProj reference $projectLocation/$projectCsProj

# create solution
dotnet new sln -n $projectName
dotnet sln $projectSln add $projectLocation/$projectCsProj
dotnet sln $projectSln add $testLocation/$testCsProj

# restore and build
dotnet restore
dotnet build