require'json'
is_local_db = true
use_filename = true

#<---		STEP 1: Load settings  -->
settings_file = 'C:\\simlink\\settings\\settings.json'
scenario_settings_file = 'C:\\simlink\\settings\\scenario.json'		# note: this is dyanmically set by Simlink
#throw 'Settings file not present in c:\simlink\settings\settings.json' unless File.exist?(settings_file)
#throw 'Settings file not present in c:\simlink\settings\scenario.json' unless File.exist?(scenario_settings_file)
begin
	settings = JSON.parse(File.read(settings_file))
	scenario_settings = JSON.parse(File.read(scenario_settings_file))
	
	throw 'Setting JSON not present or properly formed' unless File.exist?(settings_file)
	throw 'Scenario JSON config file not present' unless File.exist?(scenario_settings_file)
rescue 
	puts "FAIL:SETTINGS"
	return
end

db_loc = settings['db']
#new_scenario = scenario_settings['scenario']
network_sp = settings['network_sp']
scenario_name = scenario_settings['scenario']

	# STEP 2: Open IExchange.exe
begin
	puts "		>> Beginning scenario creation with db: #{db_loc}" 
	db=WSApplication.open db_loc, false
rescue
	puts "FAIL:IEXCHANGE"
	return
end

#STEP 3: Open the network. This will fail for local db if the user is in the db- so need to give good feedback
begin
	nno = db.model_object(network_sp)
	nno_open = nno.open
	#puts "Adding scenario '#{scenario_name}'."
rescue
	puts "FAIL:NETWORK"
	return
end


# STEP 4- Delete the scenario if it already exists
# todo: allow a flag in the future that will skip this step
begin
	check_for_scenario = true
	exists = false
	if check_for_scenario
		puts "Checking whether scenario #{scenario_name} exists"
		nno_open.scenarios do |scenario|
			exists = true if scenario == scenario_name
		end
	end
	if exists
		puts "Deleting scenario #{scenario_name}"
		nno_open.delete_scenario(scenario_name)
	end
rescue
	puts "FAIL:CHECK-OR-DELETE"
	exit
end



begin
	puts "Adding scenario '#{scenario_name}'"
	
	nno_open.add_scenario(scenario_name, nil, '')
	puts "scenario added"
	nno_open.current_scenario = scenario_name
	nno_open.close
	return
rescue
	puts "FAIL:SCENARIO"
#- i think not open, so can't close	nno_open.close
	exit
end

puts "Exiting scenario creation routine"