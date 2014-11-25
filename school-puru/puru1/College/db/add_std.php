<?php
include('../../db.php');
parse_str($_POST['add_std'], $std);
//print_r($std);
$count=mysqli_query($con,"SELECT * from clg_class where medium='".$std['clg_ads_mdm']."' AND std='".$std['clg_ads_std']."'");
$grn=mysqli_query($con,"select Gr_num from user_clg where Gr_num='".$std['clg_ads_grn']."'  OR Enroll ='".$std['clg_ads_enr']."'");
if(mysqli_num_rows($grn)==0){
	if(mysqli_num_rows($count)!=0){
		$result=mysqli_query($con,"Insert into clg_details 
			values('".trim($std['clg_ads_enr'])."',
			'".trim($std['clg_ads_grn'])."',
			'".trim($std['clg_ads_name'])."',
			'".trim($std['clg_ads_fname'])."',
			'".trim($std['clg_ads_mname'])."',
			'".trim($std['clg_ads_sex'])."',
			'".trim($std['clg_ads_dob'])."',
			'".trim($std['clg_ads_cor'])."',
			'".trim($std['clg_ads_bplc'])."',
			'".trim($std['clg_ads_cont_num'])."',
			'".trim($std['clg_ads_adrs'])."',
			'".trim($std['clg_ads_docs'])."',
			'".trim($std['clg_ads_relg'])."',
			'".trim($std['clg_ads_cast'])."',
			'".trim($std['clg_ads_sub_cast'])."',
			'".trim($std['clg_ads_ntn'])."',
			'".trim($std['clg_ads_lclg'])."',
			'".trim($std['clg_ads_prog'])."',
			'".trim($std['clg_ads_test'])."',
			'".trim($std['clg_ads_adhar'])."',			
			'".trim($std['clg_ads_class_Addm'])."',
			'".trim($std['clg_ads_class_left'])."',
			'".trim($std['clg_dt_Addm'])."',
			'".trim($std['clg_dt_left'])."',
			'".trim($std['clg_ads_res'])."',
			'".trim($std['clg_ads_tax_status'])."',
			'".date("Y-m-d")."'
			)"
		);	
			//echo ($std['clg_ads_div']);
		$result1=mysqli_query($con,"Insert into user_clg
			values('".trim($std['clg_ads_enr'])."',
			'".trim($std['clg_ads_name'])."',
			'".trim($std['clg_ads_mdm'])."',
			'".trim($std['clg_ads_cor'])."',
			'".trim($std['clg_ads_std'])."',
			'".trim($std['sch_ads_div'])."',
			'".trim($std['clg_ads_grn'])."',
			'".date("Y-m-d")."'
			)"
		);	
			$arr['status']="Details Added";
			echo json_encode($arr);
			exit;
	}
	else{
		$arr['status']="No class of this description available";
		echo json_encode($arr);
		exit;
	}
}
else{
	$arr['status']="GRN";
		echo json_encode($arr);
		exit;
}
?> 