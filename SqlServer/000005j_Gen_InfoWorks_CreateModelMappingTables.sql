/****** Object:  Table [dbo].[tblComponent]    Script Date: 4/11/2016 5:11:14 PM ******/
DROP TABLE [dbo].[tblComponent]
GO
/****** Object:  Table [dbo].[hw_weir]    Script Date: 4/11/2016 5:11:14 PM ******/
DROP TABLE [dbo].[hw_weir]
GO
/****** Object:  Table [dbo].[hw_subcatchment]    Script Date: 4/11/2016 5:11:14 PM ******/
DROP TABLE [dbo].[hw_subcatchment]
GO
/****** Object:  Table [dbo].[hw_sluice]    Script Date: 4/11/2016 5:11:14 PM ******/
DROP TABLE [dbo].[hw_sluice]
GO
/****** Object:  Table [dbo].[hw_pump]    Script Date: 4/11/2016 5:11:14 PM ******/
DROP TABLE [dbo].[hw_pump]
GO
/****** Object:  Table [dbo].[hw_node]    Script Date: 4/11/2016 5:11:14 PM ******/
DROP TABLE [dbo].[hw_node]
GO
/****** Object:  Table [dbo].[hw_conduit]    Script Date: 4/11/2016 5:11:14 PM ******/
DROP TABLE [dbo].[hw_conduit]
GO
/****** Object:  Table [dbo].[hw_conduit]    Script Date: 4/11/2016 5:11:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[hw_conduit](
	[ConduitID] [int] Identity(1,1) NOT NULL,
	[capacity] [float] NULL,
	[conduit_height] [float] NULL,
	[conduit_height_flag] [nvarchar](2) NULL,
	[conduit_length] [float] NULL,
	[conduit_length_flag] [nvarchar](2) NULL,
	[conduit_material] [nvarchar](5) NULL,
	[conduit_material_flag] [nvarchar](2) NULL,
	[conduit_width] [float] NULL,
	[conduit_width_flag] [nvarchar](2) NULL,
	[critical_sewer_category] [nvarchar](1) NULL,
	[critical_sewer_category_flag] [nvarchar](2) NULL,
	[design_group] [int] NULL,
	[design_group_flag] [nvarchar](2) NULL,
	[ds_headloss_coeff] [float] NULL,
	[ds_headloss_coeff_flag] [nvarchar](2) NULL,
	[ds_headloss_type] [nvarchar](32) NULL,
	[ds_headloss_type_flag] [nvarchar](2) NULL,
	[ds_invert] [float] NULL,
	[ds_invert_flag] [nvarchar](2) NULL,
	[ds_node_id] [nvarchar](30) NULL,
	[ds_settlement_eff] [int] NULL,
	[ds_settlement_eff_flag] [nvarchar](2) NULL,
	[excluded] [bit] NOT NULL,
	[gradient] [float] NULL,
	[ground_condition] [nvarchar](8) NULL,
	[ground_condition_flag] [nvarchar](2) NULL,
	[inflow] [float] NULL,
	[inflow_flag] [nvarchar](2) NULL,
	[link_suffix] [nvarchar](1) NOT NULL,
	[link_type] [nvarchar](6) NULL,
	[merge_details] [image] NULL,
	[merge_network_GUID] [nvarchar](40) NULL,
	[min_computational_nodes] [int] NULL,
	[min_computational_nodes_flag] [nvarchar](2) NULL,
	[notes] [nvarchar](max) NULL,
	[point_array] [image] NULL,
	[roughness_type] [nvarchar](7) NULL,
	[roughness_type_flag] [nvarchar](2) NULL,
	[sediment_depth] [float] NULL,
	[sediment_depth_flag] [nvarchar](2) NULL,
	[sediment_type] [int] NULL,
	[sediment_type_flag] [nvarchar](2) NULL,
	[sewer_reference] [nvarchar](80) NULL,
	[shape] [nvarchar](32) NULL,
	[shape_flag] [nvarchar](2) NULL,
	[site_condition] [nvarchar](8) NULL,
	[site_condition_flag] [nvarchar](2) NULL,
	[solution_model_flag] [nvarchar](2) NULL,
	[system_type] [nvarchar](10) NULL,
	[system_type_flag] [nvarchar](2) NULL,
	[taking_off_reference] [nvarchar](2) NULL,
	[taking_off_reference_flag] [nvarchar](2) NULL,
	[us_headloss_coeff] [float] NULL,
	[us_headloss_coeff_flag] [nvarchar](2) NULL,
	[us_headloss_type] [nvarchar](32) NULL,
	[us_headloss_type_flag] [nvarchar](2) NULL,
	[us_invert] [float] NULL,
	[us_invert_flag] [nvarchar](2) NULL,
	[us_node_id] [nvarchar](30) NOT NULL,
	[us_settlement_eff] [int] NULL,
	[us_settlement_eff_flag] [nvarchar](2) NULL,
	[user_number_1] [float] NULL,
	[user_number_1_flag] [nvarchar](2) NULL,
	[user_number_2] [float] NULL,
	[user_number_2_flag] [nvarchar](2) NULL,
	[user_text_1_flag] [nvarchar](2) NULL,
	[user_text_2_flag] [nvarchar](2) NULL,
	[hotlinks] [nvarchar](max) NULL,
	[asset_uid] [nvarchar](32) NULL,
	[infonet_ds_node_id] [nvarchar](30) NULL,
	[infonet_link_suffix] [nvarchar](1) NULL,
	[infonet_us_node_id] [nvarchar](30) NULL,
	[branch_id] [int] NULL,
	[branch_id_flag] [nvarchar](2) NULL,
	[user_number_3] [float] NULL,
	[user_number_3_flag] [nvarchar](2) NULL,
	[user_number_4] [float] NULL,
	[user_number_4_flag] [nvarchar](2) NULL,
	[user_number_5] [float] NULL,
	[user_number_5_flag] [nvarchar](2) NULL,
	[user_text_3_flag] [nvarchar](2) NULL,
	[user_text_4_flag] [nvarchar](2) NULL,
	[user_text_5_flag] [nvarchar](2) NULL,
	[user_text_1] [nvarchar](100) NULL,
	[user_text_2] [nvarchar](100) NULL,
	[user_text_3] [nvarchar](100) NULL,
	[user_text_4] [nvarchar](100) NULL,
	[user_text_5] [nvarchar](100) NULL,
	[base_height] [float] NULL,
	[base_height_flag] [nvarchar](2) NULL,
	[fill_material_conductivity] [float] NULL,
	[fill_material_conductivity_flag] [nvarchar](2) NULL,
	[infiltration_coeff_base] [float] NULL,
	[infiltration_coeff_base_flag] [nvarchar](2) NULL,
	[infiltration_coeff_side] [float] NULL,
	[infiltration_coeff_side_flag] [nvarchar](2) NULL,
	[porosity] [float] NULL,
	[porosity_flag] [nvarchar](2) NULL,
	[solution_model] [nvarchar](9) NULL,
	[bottom_roughness_CW] [float] NULL,
	[bottom_roughness_CW_flag] [nvarchar](2) NULL,
	[bottom_roughness_Manning] [float] NULL,
	[bottom_roughness_Manning_flag] [nvarchar](2) NULL,
	[top_roughness_CW] [float] NULL,
	[top_roughness_CW_flag] [nvarchar](2) NULL,
	[top_roughness_Manning] [float] NULL,
	[top_roughness_Manning_flag] [nvarchar](2) NULL,
	[asset_id_flag] [nvarchar](2) NULL,
	[ds_node_id_flag] [nvarchar](2) NULL,
	[us_node_id_flag] [nvarchar](2) NULL,
	[Version] [int] NOT NULL,
	[asset_id] [nvarchar](32) NULL,
	[ModelVersion] [int] NULL,
	[Description] [nvarchar](255) NULL,
	
CONSTRAINT [PK_hw_conduit] PRIMARY KEY CLUSTERED 
(
	[us_node_id], [link_suffix], [Version] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[hw_node]    Script Date: 4/11/2016 5:11:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[hw_node](
	[chamber_area] [float] NULL,
	[chamber_area_flag] [nvarchar](2) NULL,
	[chamber_floor] [float] NULL,
	[chamber_floor_flag] [nvarchar](2) NULL,
	[chamber_roof] [float] NULL,
	[chamber_roof_flag] [nvarchar](2) NULL,
	[excluded] [bit] NOT NULL,
	[flood_area_1] [int] NULL,
	[flood_area_1_flag] [nvarchar](2) NULL,
	[flood_area_2] [int] NULL,
	[flood_area_2_flag] [nvarchar](2) NULL,
	[flood_depth_1] [float] NULL,
	[flood_depth_1_flag] [nvarchar](2) NULL,
	[flood_depth_2] [float] NULL,
	[flood_depth_2_flag] [nvarchar](2) NULL,
	[flood_level] [float] NULL,
	[flood_level_flag] [nvarchar](2) NULL,
	[flood_type] [nvarchar](6) NULL,
	[flood_type_flag] [nvarchar](2) NULL,
	[floodable_area] [float] NULL,
	[floodable_area_flag] [nvarchar](2) NULL,
	[ground_level] [float] NULL,
	[ground_level_flag] [nvarchar](2) NULL,
	[node_id] [nvarchar](30) NOT NULL,
	[node_type] [nvarchar](7) NULL,
	[notes] [nvarchar](max) NULL,
	[shaft_area] [float] NULL,
	[shaft_area_flag] [nvarchar](2) NULL,
	[storage_area_array] [image] NULL,
	[storage_level_array] [image] NULL,
	[system_type] [nvarchar](10) NULL,
	[system_type_flag] [nvarchar](2) NULL,
	[user_number_1] [float] NULL,
	[user_number_1_flag] [nvarchar](2) NULL,
	[user_number_2] [float] NULL,
	[user_number_2_flag] [nvarchar](2) NULL,
	[user_text_1_flag] [nvarchar](2) NULL,
	[user_text_2_flag] [nvarchar](2) NULL,
	[x] [float] NULL,
	[x_flag] [nvarchar](2) NULL,
	[y] [float] NULL,
	[y_flag] [nvarchar](2) NULL,
	[chamber_area_additional] [float] NULL,
	[chamber_area_additional_flag] [nvarchar](2) NULL,
	[shaft_area_additional] [float] NULL,
	[shaft_area_additional_flag] [nvarchar](2) NULL,
	[base_area] [float] NULL,
	[base_area_flag] [nvarchar](2) NULL,
	[infiltration_coeff] [float] NULL,
	[infiltration_coeff_flag] [nvarchar](2) NULL,
	[perimeter] [float] NULL,
	[perimeter_flag] [nvarchar](2) NULL,
	[porosity] [float] NULL,
	[porosity_flag] [nvarchar](2) NULL,
	[hotlinks] [nvarchar](max) NULL,
	[chamber_area_add_comp] [float] NULL,
	[chamber_area_add_comp_flag] [nvarchar](2) NULL,
	[chamber_area_add_ncorrect] [float] NULL,
	[chamber_area_add_simplify] [float] NULL,
	[chamber_area_add_simplify_flag] [nvarchar](2) NULL,
	[shaft_area_add_comp] [float] NULL,
	[shaft_area_add_comp_flag] [nvarchar](2) NULL,
	[shaft_area_add_ncorrect] [float] NULL,
	[shaft_area_add_ncorrect_flag] [nvarchar](2) NULL,
	[shaft_area_add_simplify] [float] NULL,
	[shaft_area_add_simplify_flag] [nvarchar](2) NULL,
	[chamber_area_add_ncorrect_flag] [nvarchar](2) NULL,
	[asset_uid] [nvarchar](32) NULL,
	[infonet_id] [nvarchar](30) NULL,
	[head_discharge_id] [nvarchar](32) NULL,
	[n_gullies] [float] NULL,
	[n_gullies_flag] [nvarchar](2) NULL,
	[relative_stages] [bit] NOT NULL,
	[user_number_3] [float] NULL,
	[user_number_3_flag] [nvarchar](2) NULL,
	[user_number_4] [float] NULL,
	[user_number_4_flag] [nvarchar](2) NULL,
	[user_number_5] [float] NULL,
	[user_number_5_flag] [nvarchar](2) NULL,
	[user_text_3_flag] [nvarchar](2) NULL,
	[user_text_4_flag] [nvarchar](2) NULL,
	[user_text_5_flag] [nvarchar](2) NULL,
	[user_text_1] [nvarchar](100) NULL,
	[user_text_2] [nvarchar](100) NULL,
	[user_text_3] [nvarchar](100) NULL,
	[user_text_4] [nvarchar](100) NULL,
	[user_text_5] [nvarchar](100) NULL,
	[infiltratn_coeff_abv_liner_flag] [nvarchar](2) NULL,
	[infiltratn_coeff_abv_vegn] [float] NULL,
	[infiltratn_coeff_abv_vegn_flag] [nvarchar](2) NULL,
	[infiltratn_coeff_blw_liner] [float] NULL,
	[infiltratn_coeff_blw_liner_flag] [nvarchar](2) NULL,
	[liner_level] [float] NULL,
	[liner_level_flag] [nvarchar](2) NULL,
	[perimeter_array] [image] NULL,
	[vegetation_level] [float] NULL,
	[vegetation_level_flag] [nvarchar](2) NULL,
	[infiltratn_coeff_abv_liner] [float] NULL,
	[flooding_discharge_coeff] [float] NULL,
	[flooding_discharge_coeff_flag] [nvarchar](2) NULL,
	[asset_id_flag] [nvarchar](2) NULL,
	[node_id_flag] [nvarchar](2) NULL,
	[node_type_flag] [nvarchar](2) NULL,
	[Version] [int] NOT NULL,
	[asset_id] [nvarchar](32) NULL,
	[ModelVersion] [int] NULL,
	[Description] [nvarchar](255) NULL,
	
CONSTRAINT [PK_hw_node] PRIMARY KEY CLUSTERED 
(
	[node_id], [Version] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[hw_pump]    Script Date: 4/11/2016 5:11:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[hw_pump](
	[base_level] [float] NULL,
	[base_level_flag] [nvarchar](2) NULL,
	[delay] [int] NULL,
	[delay_flag] [nvarchar](2) NULL,
	[discharge] [float] NULL,
	[discharge_flag] [nvarchar](2) NULL,
	[ds_node_id] [nvarchar](30) NULL,
	[excluded] [bit] NOT NULL,
	[head_discharge_id] [nvarchar](32) NULL,
	[link_suffix] [nvarchar](1) NOT NULL,
	[link_type] [nvarchar](6) NULL,
	[maximum_flow] [float] NULL,
	[maximum_flow_flag] [nvarchar](2) NULL,
	[minimum_flow] [float] NULL,
	[minimum_flow_flag] [nvarchar](2) NULL,
	[negative_change_in_flow] [float] NULL,
	[negative_change_in_flow_flag] [nvarchar](2) NULL,
	[notes] [nvarchar](max) NULL,
	[point_array] [image] NULL,
	[positive_change_in_flow] [float] NULL,
	[positive_change_in_flow_flag] [nvarchar](2) NULL,
	[sewer_reference] [nvarchar](80) NULL,
	[switch_off_level] [float] NULL,
	[switch_off_level_flag] [nvarchar](2) NULL,
	[switch_on_level] [float] NULL,
	[switch_on_level_flag] [nvarchar](2) NULL,
	[system_type] [nvarchar](10) NULL,
	[system_type_flag] [nvarchar](2) NULL,
	[threshold] [float] NULL,
	[threshold_flag] [nvarchar](2) NULL,
	[us_node_id] [nvarchar](30) NOT NULL,
	[user_number_1] [float] NULL,
	[user_number_1_flag] [nvarchar](2) NULL,
	[user_number_2] [float] NULL,
	[user_number_2_flag] [nvarchar](2) NULL,
	[user_text_1_flag] [nvarchar](2) NULL,
	[user_text_2_flag] [nvarchar](2) NULL,
	[ds_settlement_eff] [int] NULL,
	[ds_settlement_eff_flag] [nvarchar](2) NULL,
	[us_settlement_eff] [int] NULL,
	[us_settlement_eff_flag] [nvarchar](2) NULL,
	[hotlinks] [nvarchar](max) NULL,
	[asset_uid] [nvarchar](32) NULL,
	[infonet_id] [nvarchar](40) NULL,
	[branch_id] [int] NULL,
	[branch_id_flag] [nvarchar](2) NULL,
	[user_number_3] [float] NULL,
	[user_number_3_flag] [nvarchar](2) NULL,
	[user_number_4] [float] NULL,
	[user_number_4_flag] [nvarchar](2) NULL,
	[user_number_5] [float] NULL,
	[user_number_5_flag] [nvarchar](2) NULL,
	[user_text_3_flag] [nvarchar](2) NULL,
	[user_text_4_flag] [nvarchar](2) NULL,
	[user_text_5_flag] [nvarchar](2) NULL,
	[user_text_1] [nvarchar](100) NULL,
	[user_text_2] [nvarchar](100) NULL,
	[user_text_3] [nvarchar](100) NULL,
	[user_text_4] [nvarchar](100) NULL,
	[user_text_5] [nvarchar](100) NULL,
	[electric_hydraulic_ratio] [real] NULL,
	[electric_hydraulic_ratio_flag] [nvarchar](2) NULL,
	[nominal_flow] [real] NULL,
	[nominal_flow_flag] [nvarchar](2) NULL,
	[off_delay] [int] NULL,
	[off_delay_flag] [nvarchar](2) NULL,
	[asset_id_flag] [nvarchar](2) NULL,
	[ds_node_id_flag] [nvarchar](2) NULL,
	[us_node_id_flag] [nvarchar](2) NULL,
	[Version] [int] NOT NULL,
	[asset_id] [nvarchar](32) NULL,
	[ModelVersion] [int] NULL,
	[Description] [nvarchar](255) NULL,
	
CONSTRAINT [PK_hw_pump] PRIMARY KEY CLUSTERED 
(
	[us_node_id], [link_suffix], [Version] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[hw_sluice]    Script Date: 4/11/2016 5:11:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[hw_sluice](
	[discharge_coeff] [float] NULL,
	[discharge_coeff_flag] [nvarchar](2) NULL,
	[ds_node_id] [nvarchar](30) NULL,
	[excluded] [bit] NOT NULL,
	[invert] [float] NULL,
	[invert_flag] [nvarchar](2) NULL,
	[link_suffix] [nvarchar](1) NOT NULL,
	[link_type] [nvarchar](6) NULL,
	[maximum_opening] [float] NULL,
	[maximum_opening_flag] [nvarchar](2) NULL,
	[minimum_opening] [float] NULL,
	[minimum_opening_flag] [nvarchar](2) NULL,
	[negative_speed] [float] NULL,
	[negative_speed_flag] [nvarchar](2) NULL,
	[notes] [nvarchar](max) NULL,
	[opening] [float] NULL,
	[opening_flag] [nvarchar](2) NULL,
	[point_array] [image] NULL,
	[positive_speed] [float] NULL,
	[positive_speed_flag] [nvarchar](2) NULL,
	[secondary_discharge_coeff] [float] NULL,
	[secondary_discharge_coeff_flag] [nvarchar](2) NULL,
	[sewer_reference] [nvarchar](80) NULL,
	[system_type] [nvarchar](10) NULL,
	[system_type_flag] [nvarchar](2) NULL,
	[threshold] [float] NULL,
	[threshold_flag] [nvarchar](2) NULL,
	[us_node_id] [nvarchar](30) NOT NULL,
	[user_number_1] [float] NULL,
	[user_number_1_flag] [nvarchar](2) NULL,
	[user_number_2] [float] NULL,
	[user_number_2_flag] [nvarchar](2) NULL,
	[user_text_1_flag] [nvarchar](2) NULL,
	[user_text_2_flag] [nvarchar](2) NULL,
	[width] [float] NULL,
	[width_flag] [nvarchar](2) NULL,
	[ds_settlement_eff] [int] NULL,
	[ds_settlement_eff_flag] [nvarchar](2) NULL,
	[us_settlement_eff] [int] NULL,
	[us_settlement_eff_flag] [nvarchar](2) NULL,
	[hotlinks] [nvarchar](max) NULL,
	[asset_uid] [nvarchar](32) NULL,
	[infonet_id] [nvarchar](40) NULL,
	[branch_id] [int] NULL,
	[branch_id_flag] [nvarchar](2) NULL,
	[user_number_3] [float] NULL,
	[user_number_3_flag] [nvarchar](2) NULL,
	[user_number_4] [float] NULL,
	[user_number_4_flag] [nvarchar](2) NULL,
	[user_number_5] [float] NULL,
	[user_number_5_flag] [nvarchar](2) NULL,
	[user_text_3_flag] [nvarchar](2) NULL,
	[user_text_4_flag] [nvarchar](2) NULL,
	[user_text_5_flag] [nvarchar](2) NULL,
	[user_text_1] [nvarchar](100) NULL,
	[user_text_2] [nvarchar](100) NULL,
	[user_text_3] [nvarchar](100) NULL,
	[user_text_4] [nvarchar](100) NULL,
	[user_text_5] [nvarchar](100) NULL,
	[asset_id_flag] [nvarchar](2) NULL,
	[ds_node_id_flag] [nvarchar](2) NULL,
	[us_node_id_flag] [nvarchar](2) NULL,
	[Version] [int] NOT NULL,
	[asset_id] [nvarchar](32) NULL,
	[ModelVersion] [int] NULL,
	[Description] [nvarchar](255) NULL,
	
CONSTRAINT [PK_hw_sluice] PRIMARY KEY CLUSTERED 
(
	[us_node_id], [link_suffix], [Version] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[hw_subcatchment]    Script Date: 4/11/2016 5:11:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[hw_subcatchment](
	[additional_foul_flow_flag] [nvarchar](2) NULL,
	[area_measurement_type] [nvarchar](8) NOT NULL,
	[area_measurement_type_flag] [nvarchar](2) NULL,
	[base_flow] [float] NULL,
	[base_flow_flag] [nvarchar](2) NULL,
	[boundary_array] [image] NULL,
	[catchment_dimension] [float] NULL,
	[catchment_dimension_flag] [nvarchar](2) NULL,
	[catchment_slope] [float] NULL,
	[catchment_slope_flag] [nvarchar](2) NULL,
	[connectivity] [int] NULL,
	[connectivity_flag] [nvarchar](2) NULL,
	[contributing_area] [float] NULL,
	[contributing_area_flag] [nvarchar](2) NULL,
	[excluded] [bit] NOT NULL,
	[land_use_id] [nvarchar](32) NULL,
	[node_id] [nvarchar](30) NULL,
	[notes] [nvarchar](max) NULL,
	[population] [float] NULL,
	[population_flag] [nvarchar](2) NULL,
	[rainfall_profile] [nvarchar](32) NULL,
	[rainfall_profile_flag] [nvarchar](2) NULL,
	[soil_class] [int] NULL,
	[soil_class_flag] [nvarchar](2) NULL,
	[subcatchment_id] [nvarchar](30) NOT NULL,
	[system_type] [nvarchar](10) NULL,
	[system_type_flag] [nvarchar](2) NULL,
	[total_area] [float] NULL,
	[total_area_flag] [nvarchar](2) NULL,
	[trade_flow] [float] NULL,
	[trade_flow_flag] [nvarchar](2) NULL,
	[trade_profile] [int] NULL,
	[trade_profile_flag] [nvarchar](2) NULL,
	[user_number_1] [float] NULL,
	[user_number_1_flag] [nvarchar](2) NULL,
	[user_text_1_flag] [nvarchar](2) NULL,
	[wastewater_profile] [int] NULL,
	[wastewater_profile_flag] [nvarchar](2) NULL,
	[x] [float] NULL,
	[x_flag] [nvarchar](2) NULL,
	[y] [float] NULL,
	[y_flag] [nvarchar](2) NULL,
	[ground_id] [nvarchar](32) NULL,
	[hotlinks] [nvarchar](max) NULL,
	[curve_number] [float] NULL,
	[curve_number_flag] [nvarchar](2) NULL,
	[area_absolute_1] [float] NULL,
	[area_absolute_10] [float] NULL,
	[area_absolute_10_flag] [nvarchar](2) NULL,
	[area_absolute_11] [float] NULL,
	[area_absolute_11_flag] [nvarchar](2) NULL,
	[area_absolute_12] [float] NULL,
	[area_absolute_12_flag] [nvarchar](2) NULL,
	[area_absolute_1_flag] [nvarchar](2) NULL,
	[area_absolute_2] [float] NULL,
	[area_absolute_2_flag] [nvarchar](2) NULL,
	[area_absolute_3] [float] NULL,
	[area_absolute_3_flag] [nvarchar](2) NULL,
	[area_absolute_4] [float] NULL,
	[area_absolute_4_flag] [nvarchar](2) NULL,
	[area_absolute_5] [float] NULL,
	[area_absolute_5_flag] [nvarchar](2) NULL,
	[area_absolute_6] [float] NULL,
	[area_absolute_6_flag] [nvarchar](2) NULL,
	[area_absolute_7] [float] NULL,
	[area_absolute_7_flag] [nvarchar](2) NULL,
	[area_absolute_8] [float] NULL,
	[area_absolute_8_flag] [nvarchar](2) NULL,
	[area_absolute_9] [float] NULL,
	[area_absolute_9_flag] [nvarchar](2) NULL,
	[user_number_2] [float] NULL,
	[user_number_2_flag] [nvarchar](2) NULL,
	[user_text_2_flag] [nvarchar](2) NULL,
	[user_number_3] [float] NULL,
	[user_number_3_flag] [nvarchar](2) NULL,
	[user_number_4] [float] NULL,
	[user_number_4_flag] [nvarchar](2) NULL,
	[user_number_5] [float] NULL,
	[user_number_5_flag] [nvarchar](2) NULL,
	[user_text_3_flag] [nvarchar](2) NULL,
	[user_text_4_flag] [nvarchar](2) NULL,
	[user_text_5_flag] [nvarchar](2) NULL,
	[user_text_1] [nvarchar](100) NULL,
	[user_text_2] [nvarchar](100) NULL,
	[user_text_3] [nvarchar](100) NULL,
	[user_text_4] [nvarchar](100) NULL,
	[user_text_5] [nvarchar](100) NULL,
	[link_suffix] [nvarchar](1) NULL,
	[unit_hydrograph_id] [nvarchar](32) NULL,
	[snow_pack_id] [nvarchar](32) NULL,
	[area_percent_1] [float] NULL,
	[area_percent_10] [float] NULL,
	[area_percent_10_flag] [nvarchar](2) NULL,
	[area_percent_11] [float] NULL,
	[area_percent_11_flag] [nvarchar](2) NULL,
	[area_percent_12] [float] NULL,
	[area_percent_12_flag] [nvarchar](2) NULL,
	[area_percent_1_flag] [nvarchar](2) NULL,
	[area_percent_2] [float] NULL,
	[area_percent_2_flag] [nvarchar](2) NULL,
	[area_percent_3] [float] NULL,
	[area_percent_3_flag] [nvarchar](2) NULL,
	[area_percent_4] [float] NULL,
	[area_percent_4_flag] [nvarchar](2) NULL,
	[area_percent_5] [float] NULL,
	[area_percent_5_flag] [nvarchar](2) NULL,
	[area_percent_6] [float] NULL,
	[area_percent_6_flag] [nvarchar](2) NULL,
	[area_percent_7] [float] NULL,
	[area_percent_7_flag] [nvarchar](2) NULL,
	[area_percent_8] [float] NULL,
	[area_percent_8_flag] [nvarchar](2) NULL,
	[area_percent_9] [float] NULL,
	[area_percent_9_flag] [nvarchar](2) NULL,
	[land_use_id_flag] [nvarchar](2) NULL,
	[time_of_concentration] [float] NULL,
	[time_of_concentration_flag] [nvarchar](2) NULL,
	[base_time] [float] NULL,
	[base_time_flag] [nvarchar](2) NULL,
	[time_to_peak] [float] NULL,
	[time_to_peak_flag] [nvarchar](2) NULL,
	[uh_definition] [nvarchar](14) NULL,
	[uh_definition_flag] [nvarchar](2) NULL,
	[tc_time_to_peak_factor] [float] NULL,
	[tc_time_to_peak_factor_flag] [nvarchar](2) NULL,
	[tc_timestep_factor] [float] NULL,
	[tc_timestep_factor_flag] [nvarchar](2) NULL,
	[node_id_flag] [nvarchar](2) NULL,
	[subcatchment_id_flag] [nvarchar](2) NULL,
	[Version] [int] NOT NULL,
	[additional_foul_flow] [float] NULL,
	[ModelVersion] [int] NULL,
	[Description] [nvarchar](255) NULL,
	
CONSTRAINT [PK_hw_subcatchment] PRIMARY KEY CLUSTERED 
(
	[subcatchment_id], [Version] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[hw_weir]    Script Date: 4/11/2016 5:11:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[hw_weir](
	[crest] [float] NULL,
	[crest_flag] [nvarchar](2) NULL,
	[discharge_coeff] [float] NULL,
	[discharge_coeff_flag] [nvarchar](2) NULL,
	[ds_node_id] [nvarchar](30) NULL,
	[excluded] [bit] NOT NULL,
	[height] [float] NULL,
	[height_flag] [nvarchar](2) NULL,
	[link_suffix] [nvarchar](1) NOT NULL,
	[link_type] [nvarchar](6) NULL,
	[maximum_value] [float] NULL,
	[maximum_value_flag] [nvarchar](2) NULL,
	[minimum_value] [float] NULL,
	[minimum_value_flag] [nvarchar](2) NULL,
	[negative_speed] [float] NULL,
	[negative_speed_flag] [nvarchar](2) NULL,
	[notes] [nvarchar](max) NULL,
	[point_array] [image] NULL,
	[positive_speed] [float] NULL,
	[positive_speed_flag] [nvarchar](2) NULL,
	[secondary_discharge_coeff] [float] NULL,
	[secondary_discharge_coeff_flag] [nvarchar](2) NULL,
	[sewer_reference] [nvarchar](80) NULL,
	[system_type] [nvarchar](10) NULL,
	[system_type_flag] [nvarchar](2) NULL,
	[threshold] [float] NULL,
	[threshold_flag] [nvarchar](2) NULL,
	[us_node_id] [nvarchar](30) NOT NULL,
	[user_number_1] [float] NULL,
	[user_number_1_flag] [nvarchar](2) NULL,
	[user_number_2] [float] NULL,
	[user_number_2_flag] [nvarchar](2) NULL,
	[user_text_1_flag] [nvarchar](2) NULL,
	[user_text_2_flag] [nvarchar](2) NULL,
	[width] [float] NULL,
	[width_flag] [nvarchar](2) NULL,
	[ds_settlement_eff] [int] NULL,
	[ds_settlement_eff_flag] [nvarchar](2) NULL,
	[us_settlement_eff] [int] NULL,
	[us_settlement_eff_flag] [nvarchar](2) NULL,
	[length] [float] NULL,
	[length_flag] [nvarchar](2) NULL,
	[notch_angle] [float] NULL,
	[notch_angle_flag] [nvarchar](2) NULL,
	[notch_height] [float] NULL,
	[notch_height_flag] [nvarchar](2) NULL,
	[notch_width] [float] NULL,
	[notch_width_flag] [nvarchar](2) NULL,
	[number_of_notches] [int] NULL,
	[number_of_notches_flag] [nvarchar](2) NULL,
	[hotlinks] [nvarchar](max) NULL,
	[asset_uid] [nvarchar](32) NULL,
	[infonet_id] [nvarchar](40) NULL,
	[branch_id] [int] NULL,
	[branch_id_flag] [nvarchar](2) NULL,
	[user_number_3] [float] NULL,
	[user_number_3_flag] [nvarchar](2) NULL,
	[user_number_4] [float] NULL,
	[user_number_4_flag] [nvarchar](2) NULL,
	[user_number_5] [float] NULL,
	[user_number_5_flag] [nvarchar](2) NULL,
	[user_text_3_flag] [nvarchar](2) NULL,
	[user_text_4_flag] [nvarchar](2) NULL,
	[user_text_5_flag] [nvarchar](2) NULL,
	[user_text_1] [nvarchar](100) NULL,
	[user_text_2] [nvarchar](100) NULL,
	[user_text_3] [nvarchar](100) NULL,
	[user_text_4] [nvarchar](100) NULL,
	[user_text_5] [nvarchar](100) NULL,
	[asset_id_flag] [nvarchar](2) NULL,
	[ds_node_id_flag] [nvarchar](2) NULL,
	[us_node_id_flag] [nvarchar](2) NULL,
	[Version] [int] NOT NULL,
	[asset_id] [nvarchar](32) NULL,
	[ModelVersion] [int] NULL,
	[Description] [nvarchar](255) NULL,
	
CONSTRAINT [PK_hw_weir] PRIMARY KEY CLUSTERED 
(
	[us_node_id], [link_suffix], [Version] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblComponent]    Script Date: 4/11/2016 5:11:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblComponent](
	[ComponentID] [int] Identity(1,1) NOT NULL,
	[ComponentType_FK] [int] NULL,
	[ComponentLabel] [nvarchar](255) NULL,
	[ComponentDescription] [nvarchar](255) NULL,
	[ModelVersion] [int] NULL,
	[Description] [nvarchar](255) NULL,
	
CONSTRAINT [PK_tblComponent] PRIMARY KEY CLUSTERED 
(
	[ComponentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.05j', 'Creating schema for InfoWorks model mapping tables'); 