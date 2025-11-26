# Build the entire solution to ensure all dependencies (like Bots) are ready
Write-Host "Building the entire solution..." -ForegroundColor Cyan
dotnet build SplBotHub.sln

# Check if build was successful
if ($LASTEXITCODE -eq 0) {
    Write-Host "Build successful. Starting BotHub..." -ForegroundColor Green
    # Run the main project without rebuilding, as we just built the solution
    Set-Location "BotHub"
    dotnet run --project "BotHub.csproj" --no-build
    Set-Location ..
}
else {
    Write-Host "Build failed. Please check the errors above." -ForegroundColor Red
    exit 1
}
