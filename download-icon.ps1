# Скрипт для скачивания иконки Zabbix
$iconUrl = "https://www.zabbix.com/favicon.ico"
$iconPath = Join-Path $PSScriptRoot "icon.ico"

Write-Host "Скачивание иконки Zabbix..." -ForegroundColor Green
try {
    Invoke-WebRequest -Uri $iconUrl -OutFile $iconPath -ErrorAction Stop
    Write-Host "Иконка успешно скачана: $iconPath" -ForegroundColor Green
    Write-Host "Теперь можно собрать проект: dotnet build" -ForegroundColor Yellow
}
catch {
    Write-Host "Ошибка при скачивании иконки: $_" -ForegroundColor Red
    Write-Host "Вы можете скачать иконку вручную с https://www.zabbix.com/favicon.ico" -ForegroundColor Yellow
    Write-Host "и сохранить её как icon.ico в папке проекта" -ForegroundColor Yellow
}


