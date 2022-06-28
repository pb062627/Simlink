# -*- coding: utf-8 -*-
"""
Created on Sun Nov 27 18:16:59 2016

@author: Mason
"""

import pyodbc

#cnxn = pyodbc.connect(r'DRIVER={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\Mason\Documents\WORK\MSD_Cinci\analysis\simlink\simlink_millCreek.accdb;')
#crsr = cnxn.cursor()
#for table_name in crsr.tables(tableType='TABLE'):
#    print(table_name)
    
db = r'C:\Users\Mason\Documents\WORK\MSD_Cinci\analysis\simlink\simlink_millCreek.accdb'     # replace w your version 
sql_event_val = 'select totalval from tblResultTS_EventSummary_Detail where (EventSummary_ID = ?)'
sql_event_maxval = 'select maxval from tblResultTS_EventSummary_Detail where (EventSummary_ID = ?)'
sql_event_all_byeval = 'SELECT ScenarioID_FK, EventSummary_ID, EventDuration, EventBeginPeriod, MaxVal, TotalVal, SubEventThresholdPeriods, EventNo FROM tblResultTS_EventSummary_Detail WHERE (((ScenarioID_FK) In (select scenarioid from tblScenario where EvalGroupID = ?)))'

def GetSQL(sltype):
    sql = "undefined"    
    if sltype == "event":
        sql = sql_event_val
    elif sltype == "event_max":
        sql = sql_event_maxval
    elif sltype == "sql_event_all_byeval":
        sql = sql_event_all_byeval
    return sql 

def GetVals(sltype, param):
    conn = pyodbc.connect(r'DRIVER={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=' + db + ';')
    cursor = conn.cursor()    
    lstReturn = []
    sql = GetSQL(sltype)
    cursor.execute(sql,param)
    rows = cursor.fetchall()
    for row in rows:
        lstReturn.append(row[0])
    conn.close()
    return lstReturn

a=GetVals("event",[2119])
b=GetVals("event_max",[2119])