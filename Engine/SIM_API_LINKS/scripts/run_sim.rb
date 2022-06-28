require 'date'
require 'json'

# get a dictionary of parameters from an existing run
def copy_run_paramters(run)
    puts "Copying run parameters from existing run '#{run.name}'"
    parameters_to_copy = [
     'Always Use Final State', 'Buildup Time', 'CheckPumps', 'Comment', 'ConcentrationWarning', 'Depth',
     'Depth_threshold', 'DontApplyRainfallSmoothing', 'DontLogModeSwitches', 'DontLogRTCRuleChanges',
     'DontOutputRTCState', 'Duration', 'DurationUnit', 'DWFDefinition', 'DWFModeResults', 'DWFMultiplier',
     'End Duration', 'End Time', 'EveryNode', 'EveryOutflow', 'EverySubcatchment', 'ExitOnFailedInit',
     'ExitOnFailedInitCompletion', 'GaugeMultiplier', 'Gauges', 'GetStartTimeFromRainEvent', 'Ground Infiltration',
     'IncludeBaseFlow', 'IncludeLevel', 'IncludeNode', 'IncludeOutflow', 'IncludeRainfall', 'IncludeRTC',
     'IncludeRunoff', 'Inflow', 'InitCompletionMinutes', 'Initial Conditions 2D', 'InundationMapDepthThreshold',
     'Level', 'LevelLag', 'LevelThreshold', 'MaxVelocity', 'Minor Timestep 2D', 'Momentum', 'NodeLag', 'NodeThreshold',
     'OutflowLag', 'OutflowThreshold', 'Pipe Sediment Data', 'Pollutant Graph', 'QM Dependent Fractions',
     'QM Hydraulic Feedback', 'QM Model Macrophytes', 'QM Multiplier', 'QM Native Washoff Routing', 'QM Oxygen Demand',
     'RainfallLag', 'RainfallThreshold', 'RainType', 'ReadSubeventNAPIAndAntecedDepth', 'ReadSubeventParams',
     'Regulator', 'ResultsMultiplier', 'RTCLag', 'RTCRulesOverride', 'RunoffOnly', 'Save Final State', 'Sim',
     'SpillCorrection', 'Start Time', 'StopOnEndOfTimeVaryingData', 'StorePRN', 'StormDefinition', 'SubcatchmentLag',
     'SubcatchmentThreshold', 'Theta', 'Time_lag', 'TimeStep', 'timestep_stability_control', 'TimestepLog',
     'Trade Waste', 'Use_local_steady_state', 'UseGPU', 'UseQM', 'Velocity', 'Velocity_threshold', 'VelocityWarning',
     'VolumeBalanceWarning', 'Waste Water', 'Working'
    ]
    parameters = {}
    parameters_to_copy.each do |p|
        parameters[p] = run[p]
    end
    parameters
end

def run_simulation(run, binary_path, binary_path_ts = "undefined", retries=0)
    puts 'Running simulation...'
    sim = run.children[0]
    sim.run_ex('*', 0)
    puts 'Running simulation finished.'

    if sim.status != "Success"
        puts "Simulation status: #{sim.status}."
        if (retries > 0)
            # re-try the solve
            puts 'Oops.. Retrying simulation #{retries}.'
            run_simulation(run, binary_path, retries - 1)
        else
            WSApplication.set_exit_code(1)
        end
    else
        puts "Simulation status: '#{sim.status}'. Success substatus: '#{sim.success_substatus}'"

        if sim.success_substatus == "Incomplete"
            puts "Simulation incomplete - results will not be exported."
            WSApplication.set_exit_code(1)
        else
		# todo : provide way to overide requested output
            attributes_to_export = [
              [ "Node", ["flooddepth", "floodvolume", "volume", "depnod"] ],
              [ "Link", ["ds_flow", "ds_totalhead", "us_totalhead"] ]
            ]
	
	    if binary_path !="undefined"
		sim.max_results_binary_export(nil, attributes_to_export, binary_path)
		puts "Exporting summary results to: #{binary_path}"		
	    end

	    if binary_path_ts !="undefined"
		sim.results_binary_export(nil, attributes_to_export, binary_path_ts)
		puts "Exporting binary timeseries results to: #{binary_path_ts}"		
	    end
        end
    end
end

#<---		STEP 1: Load settings  -->
settings_file = 'C:\\simlink\\settings\\settings.json'
scenario_settings_file = 'C:\\simlink\\settings\\scenario.json'		# note: this is dyanmically set by Simlink
bin_output_ts = 'C:\\simlink\\icm_output\\icm_output_ts.bin'
bin_output = 'C:\\simlink\\icm_output\\icm_output.bin'

begin
	settings = JSON.parse(File.read(settings_file))
	scenario_settings = JSON.parse(File.read(scenario_settings_file))
rescue 
	puts "FAIL:SETTINGS"
	return
end

db_loc = settings['db']

	# STEP 2: Open IExchange.exe
begin
	puts "Using icm db defined in input args #{db_loc}" 
	db=WSApplication.open db_loc, false
rescue
	puts "FAIL:IEXCHANGE"
	return
end

	#<<-----------------  STEP 3-  Get parameters from the old run --------------------------->
run_sp = settings['run_sp']
old_run = db.model_object(run_sp)
throw 'run object not found' unless old_run
parameters = copy_run_paramters(old_run)

	#<<-----------------  STEP 4 Setup a new run --------------------------->
rainfalls_and_flows = []      #rainfalls_and_flows = []
parent = db.model_object_from_type_and_id old_run.parent_type, old_run.parent_id

scenario_name = scenario_settings['scenario_name']
network_path = settings['network_sp']							#db.model_object base_network_sp_as_arg
modified_commit_id = scenario_settings['scenario_commit_id']
existing_run = parent.find_child_model_object('Run', scenario_name)

if existing_run
    puts "Deleting existing run"
    existing_run.delete
end

puts "Creating Run #{scenario_name}"
run = parent.new_run(scenario_name, network_path, modified_commit_id, rainfalls_and_flows, scenario_name, parameters)

run_simulation(run, bin_output, bin_output_ts, 1)

if false
	puts "deleting run"
	run.delete
end
