REM Navigate to the location of the VISSIM Exe OR have this path in your system environment path variables
REM cd C:\Program Files\PTV Vision\PTV Vissim 9\Exe

REM Execute the running of the model - Scen 20069
REM -q runs it in quick mode - doesn't shows the cars moving around
REM -s closes the model after the simulation and saves to the location specified
VISSIM90.exe -q -s C:\temp "\\hchfpp01\Groups\WBG\Optimization\Proj\I270_Transportation\187\20069\Roundabout London_20069.inpx"

REM Execute the running of the model - Scen 20070
VISSIM90.exe -q -s C:\temp "\\hchfpp01\Groups\WBG\Optimization\Proj\I270_Transportation\187\20070\Roundabout London_20070.inpx"

pause