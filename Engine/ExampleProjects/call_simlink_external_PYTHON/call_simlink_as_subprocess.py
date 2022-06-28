    # -*- coding: utf-8 -*-
"""
Created on Thu Oct 27 17:07:45 2016

this is a test file for calling simlink processes from an external program (in this case, python)
"""

import os
import subprocess

objective  =666666666666    # use lrge positive val so not confused with a good solution
select_case = 2   # select which test you wish to run manually (todo override as arg w default)
# change loc of simlink exe, or add to path for simpler call
work_dir = r'C:\Users\Mason\Documents\Optimization\SimLink\SimLink_v2.6\simlink2'
simlink_exe= work_dir + r'\Engine\SimLink_CLI\obj\Debug\simlink.exe'
config_file = work_dir + r'\Engine\ExampleProjects\call_simlink_external_PYTHON\epanet_config.xml'
dna = '2.2.2.3.3.3.3.3.3.3.3.3.3.3.3.3.3.3.3.3.3.3'  # set manually as test

if select_case==1:
    sl_args = ' -test -silent'
elif select_case==2:   # run a simlink scenario
    sl_args = ' -config ' + config_file + ' -run -dna ' + dna

print "begin simlink process" 
proc = subprocess.Popen(simlink_exe + sl_args, stdout=subprocess.PIPE)

ncounter = 0
for line in proc.stdout:       
    #print line
    if '#Objective' in line:
        objective =  line.partition(':')[2]
    
print "objective (python):" + objective

