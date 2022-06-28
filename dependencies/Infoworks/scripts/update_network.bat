IF %ERRORLEVEL% NEQ 0 GOTO ProcessErr

"C:\Program Files\Innovyze Workgroup Client 8.5\IExchange.exe" C:\simlink\scripts\update_network.rb /icm

exit

:ProcessErr

echo UPDATE_NETWORK_BAT