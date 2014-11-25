<?php
include('../../db.php');
parse_str($_POST['add_std'], $std);
//print_r($std);
$count=mysqli_query($con,"SELECT * from sch_class where medium='".$std['sch_ads_mdm']."' AND std='".$std['sch_ads_std']."'");
$grn=mysqli_query($con,"select Gr_num from user_sch where Gr_num='".$std['sch_ads_grn']."' OR Enroll ='".$std['sch_ads_enr']."'");
if(mysqli_num_rows($grn)==0){
	if(mysqli_num_rows($count)!=0){
		$result=mysqli_query($con,"Insert into sch_details 
			values('".trim($std['sch_ads_enr'])."',
			'".trim($std['sch_ads_grn'])."',
			'".trim($std['sch_ads_name'])."',
			'".trim($std['sch_ads_fname'])."',
			'".trim($std['sch_ads_mname'])."',
			'".trim($std['sch_ads_sex'])."',
			'".trim($std['sch_ads_dob'])."',
			'".trim($std['sch_ads_bplc'])."',
			'".trim($std['sch_ads_cont_num'])."',
			'".trim($std['sch_ads_adrs'])."',
			'".trim($std['sch_ads_docs'])."',
			'".trim($std['sch_ads_relg'])."',
			'".trim($std['sch_ads_cast'])."',
			'".trim($std['sch_ads_sub_cast'])."',
			'".trim($std['sch_ads_ntn'])."',
			'".trim($std['sch_ads_lsch'])."',
			'".trim($std['sch_ads_prog'])."',
			'".trim($std['sch_ads_test'])."',
			'".trim($std['sch_ads_class_Addm'])."',
			'".trim($std['sch_ads_adhar'])."',
			'".trim($std['sch_ads_class_left'])."',
			'".trim($std['sch_dt_Addm'])."',
			'".trim($std['sch_dt_left'])."',
			'".trim($std['sch_ads_res'])."',
			'".trim($std['sch_ads_tax_status'])."',
			'".date("Y-m-d")."'
			)"
		);	
			//echo ($std['sch_ads_div']);
		$result1=mysqli_query($con,"Insert into user_sch
			values('".trim($std['sch_ads_enr'])."',
			'".trim($std['sch_ads_name'])."',
			'".trim($std['sch_ads_mdm'])."',
			'".trim($std['sch_ads_std'])."',
			'".trim($std['sch_ads_div'])."',
			'".trim($std['sch_ads_grn'])."',
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