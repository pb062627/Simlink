for %%G in (*.sql) do sqlcmd /S PHLDNRZ9G2\SQLEXPRESS2014 /d Simlink_Template_FromScripts_EPANETDemo1 -E -i"%%G"
pause
