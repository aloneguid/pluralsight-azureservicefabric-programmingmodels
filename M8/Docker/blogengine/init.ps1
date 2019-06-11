# Remove existing default web site files
remove-item C:\inetpub\wwwroot\iisstart.*

# Ensure write permissions over web app project files
icacls C:\inetpub\wwwroot /grant Everyone:F /t /q