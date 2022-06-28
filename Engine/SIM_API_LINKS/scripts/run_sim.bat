IF %ERRORLEVEL% NEQ 0 GOTO ProcessErr

"C:\Program Files\Innovyze Workgroup Client 8.5\IExchange.exe" C:\simlink\scripts\run_sim.rb /icm
exit

:ProcessErr
echo FAIL:RUN_SIM_BAT