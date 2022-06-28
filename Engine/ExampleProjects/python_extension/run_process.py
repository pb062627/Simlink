# -*- coding: utf-8 -*-
"""
Created on Tue Dec 26 20:07:07 2017

@author: Mason
"""

#import pandas as pd

import sys
import csv
import pandas as pd

#args = sys.argv[1]
file_in = r'c:\temp\python_args.csv'  # file to read in data from simlin
file_out = r'c:\temp\python_out.csv'  # file to write data from python (phase 1: last line only used- single val)
                                                                        #phase 2: read in array (not implemented in simlink)
### grab the data from the file
def read_file():
    with open(file_in) as csvfile:
        reader = csv.reader(csvfile, delimiter=',', quotechar='|')
        for row in reader:
            x = (', '.join(row))
    return x

def create_df(args):
    slines = args.split(';')
    count = len(slines)
    
    lst_type=[]
    lst_scenario=[]
  #  lst_record=[]
    lst_label =[]
    lst_val=[]
    lst_varcode=[]
    columns=['type','scenario','label','val','varcode']
    
    # loop over the lines and extract the data...x
    for row in slines:   #i =1; i<count;i++:         # begin with the second line; first line stores scenario/eval and other info
       if(len(row)>0):         # skip extra line inserted for some reason      
           lines = row.split(',') # split the current line
           lst_type.append(lines[0].strip())
           lst_scenario.append(lines[1].strip())
           lst_label.append(lines[2].strip())
           lst_val.append(float(lines[3].strip()))
           lst_varcode.append(float(lines[4].strip()))
       #lst_type.append(lines[4])
      # lst_type.append(lines[5])
    df = pd.DataFrame(zip(lst_type,lst_scenario,lst_label,lst_val,lst_varcode),columns=columns)
    return df
    
    
    # write a value to the output file (to be read by simlink)
def write_to_file(val):
    with open(file_out, 'wb') as csvfile:
        writer = csv.writer(csvfile, delimiter=' ',
                                quotechar='|', quoting=csv.QUOTE_MINIMAL)
        writer.writerow([str(val)])
# begin the main function here...
args = read_file()
df = create_df(args)
sum_val = df['val'].sum()
print sum_val
write_to_file(sum_val)
