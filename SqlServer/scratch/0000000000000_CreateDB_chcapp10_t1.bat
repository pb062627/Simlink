for %%G in (*.sql) do sqlcmd /S Chcapp10\sqlexpress /d simlink_test1 -E -i"%%G"
pause
