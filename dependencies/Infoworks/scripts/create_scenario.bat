IF %ERRORLEVEL% NEQ 0 GOTO ProcessErr

"C:\Program Files\Innovyze Workgroup Client 8.5\IExchange.exe" C:\simlink\scripts\create_scenario.rb /icm
exit

:ProcessErr
echo FAIL:SCENARIO_CREATE_BAT