# Zabbix Monitor Viewer

Простая программа на C# для просмотра Zabbix монитора.

## Требования

- .NET 8.0 SDK или выше
- Visual Studio 2022 или Visual Studio Code (с расширением C#)
- Microsoft Edge WebView2 Runtime (обычно устанавливается автоматически с Windows или можно скачать с https://developer.microsoft.com/microsoft-edge/webview2/)

## Добавление иконки

Для добавления иконки Zabbix к приложению:

1. **Скачайте иконку Zabbix** в формате `.ico`:
   - Можно использовать официальный логотип Zabbix
   - Или скачать с ресурсов: https://www.iconfinder.com/search?q=zabbix или https://icons8.com/icons/set/zabbix
   - Конвертируйте PNG в ICO, если нужно (можно использовать онлайн-конвертеры)

2. **Поместите файл** `icon.ico` в корневую папку проекта (рядом с `ZabbixMonitor.csproj`)

3. **Пересоберите проект** - иконка будет автоматически включена в приложение

Примечание: Если файл `icon.ico` отсутствует, приложение будет работать без иконки (используется стандартная иконка Windows).

## Сборка и запуск

### Через Visual Studio:
1. Откройте файл `ZabbixMonitor.csproj` в Visual Studio
2. Нажмите F5 для запуска или Ctrl+Shift+B для сборки

### Через командную строку:
```bash
# Восстановление зависимостей (если нужно)
dotnet restore

# Сборка проекта
dotnet build

# Запуск приложения
dotnet run

# Сборка исполняемого файла (.exe)

## Вариант 1: Один файл (рекомендуется)
```bash
dotnet publish -c Release -r win-x64 --self-contained -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
```

## Вариант 2: Один файл без зависимостей (требует установленный .NET Runtime)
```bash
dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true
```

## Вариант 3: С зависимостями (папка с файлами)
```bash
dotnet publish -c Release -r win-x64 --self-contained
```

После сборки исполняемый файл `ZabbixMonitor.exe` будет находиться в папке:
- **Вариант 1 и 2**: `bin\Release\net8.0-windows\win-x64\publish\ZabbixMonitor.exe`
- **Вариант 3**: `bin\Release\net8.0-windows\win-x64\publish\`

### Примечания:
- **Вариант 1** создает один большой .exe файл со всеми зависимостями (~100-150 МБ), который можно запускать на любом Windows компьютере без установки .NET
- **Вариант 2** создает один .exe файл (~5-10 МБ), но требует установленный .NET 8.0 Runtime на целевом компьютере
- **Вариант 3** создает папку с несколькими файлами, включая .exe и все зависимости

## Особенности

- При запуске запрашивает адрес Zabbix (подстановкой служит `https://`, можно заменить на нужный)
- Блокирует навигацию за пределы домена указанного адреса
- Блокирует открытие внешних ссылок в новых окнах
- Приложение открывается в полноэкранном режиме для удобного просмотра монитора
- Использует Microsoft Edge WebView2 (Chromium) для отображения веб-контента
- Поддерживает современные веб-стандарты и HTTPS сертификаты

