<?php
parse_str($_POST['adm_upd'],$arr);
//print_r($arr);
include('../../db.php');
$result=mysqli_query($con,"SELECT * from clg_class where medium='".$arr['adm_upd_mdm']."' AND course='".$arr['adm_upd_cor']."' AND std='".$arr['adm_upd_std']."'");
//echo("SELECT class_id from sch_class where medium='".$arr['adm_upd_mdm']."' AND std='".$arr['adm_upd_std']."'"); 
if(mysqli_num_rows($result)!=0){
	$q=mysqli_query($con,"select Medium, Std, course from user_clg where Gr_num='".$_POST['adm_grn']."'");
	$f=mysqli_fetch_row($q);
	$query="Update user_clg
		SET	Name='".$arr['adm_upd_name']."',
		Medium='".$arr['adm_upd_mdm']."',
		Std='".$arr['adm_upd_std']."',
		Section='".$arr['adm_upd_div']."',
		Course='".$arr['adm_upd_cor']."',
		Gr_num='".$arr['adm_upd_grn']."'
		where GR_num='".$_POST['adm_grn']."'";
		$query1="Update clg_details 
		SET Name='".$arr['adm_upd_name']."', 
		Gr_num='".$arr['adm_upd_grn']."',
		Enroll='".$arr['adm_upd_enroll']."',
		Course='".$arr['adm_upd_cor']."',
		f_name='".$arr['adm_upd_fname']."',
		cont_num='".$arr['adm_upd_cont']."',
		m_name='".$arr['adm_upd_mname']."',
		sex='".$arr['adm_upd_sex']."',
		DOB='".$arr['adm_upd_dob']."',
		birth_place='".$arr['adm_upd_bplace']."',
		address='".$arr['adm_upd_adrs']."',
		docs='".$arr['adm_upd_docs']."',
		religion='".$arr['adm_upd_rel']."',
		caste='".$arr['adm_upd_cast']."',
		sub_caste='".$arr['adm_upd_scast']."',
		nationality='".$arr['adm_upd_nati']."',
		last_school='".$arr['adm_upd_lsch']."',
		progress='".$arr['adm_upd_prog']."',
		caste='".$arr['adm_upd_cast']."',
		status='".$arr['adm_upd_tsta']."'
		where Gr_num='".$_POST['adm_grn']."'
		
	";
	if(!($arr['adm_upd_mdm']==$f[0] && $arr['adm_upd_std']==$f[1]) && $arr['adm_upd_std']==$f[2])
		mysqli_query($con,"Delete from clg_tran where Gr_num='".$arr['adm_upd_grn']."' ");	
	mysqli_query($con,$query);
	mysqli_query($con,$query1);
	//echo $query;
	//echo($_SESSION['enroll']);
	$str['status']='suceed';
	echo json_encode($str);
	exit;
}
else{
	$str['status']='failed';
	echo json_encode($str);
	exit;
}
?>