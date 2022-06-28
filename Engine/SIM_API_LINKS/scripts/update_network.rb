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
rescue 
	puts "FAIL:SETTINGS"
end

csv_for_update = "c:\\simlink\\bin\\update.csv"

db_loc = settings['db']
new_scenario = scenario_settings['scenario']
network_sp = settings['network_sp']
scenario_name = scenario_settings['scenario']			# IMPORTANT- this must have been created using the create_scnenario script

	# STEP 2: Open IExchange.exe
begin
	puts "		>> Beginning ICM network update process with db: #{db_loc}" 
	db=WSApplication.open db_loc, false
rescue
	puts "FAIL:IEXCHANGE"
	return
end	
	
	# STEP 3: Set the active scenario name
begin
	puts "		>> Opening the base network"
	nno = db.model_object(network_sp)
	nno_open = nno.open
	puts "		>> Setting current scenario to : #{scenario_name}" 
	
	nno_open.current_scenario = scenario_name
rescue
	puts:"FAIL:SCENARIO_DOES_NOT_EXIST"
	exit
end
	# STEP 4: Update the network with the CSV
begin
	puts "		>> Updating network with CSV file : #{csv_for_update}" 
	nno_open.csv_import(csv_for_update, nil)
	# do we need to validate right now?
	validations = nno_open.validate scenario_name
#	validations.each do |v|
#	    puts "Validation: #{v.type}, #{v.field_description}, #{v.message}, #{v.object_id}"
#	end
#	nno_open.close
rescue 
	puts:"FAIL:UPDATE"
	# should this be kept around? nno_open.close
	exit
end
begin
	if (0!=0)   # validations.error_count != 0)
	    # errors occurred - record a failure message and return a non-zero exit code
	    log "Network validation failed. Removing scenario '#{scenario_name}'."
	    nno_open.delete_scenario(scenario_name)
	    nno_open.close
	    WSApplication.set_exit_code 1
	else
#	    nno_open = network.open
#	    nno_open.current_scenario = scenario_name   
#	    nno_open.close

	    # commit the updated network as a new version
	    modified_commit_id = nno_open.commit "Added new scenario: #{scenario_name}"
	    
	    puts "New commit id: #{modified_commit_id}" 
	    nno_open.close

	    # save some details that are required for the solve script
	    scenario_settings['scenario_commit_id'] = modified_commit_id
	    puts scenario_settings
	    File.write(scenario_settings_file, scenario_settings.to_json)

	    WSApplication.set_exit_code 0
	end
rescue
	puts:"FAIL:CLOSE"
	#nno_open.close
end