<?php
include('../../db.php');

parse_str($_POST['adm_cusr'], $set);
$check=mysqli_query($con,"select * from admin_sch where uname='".$set['adm_cusr_uname']."'");
if(mysqli_num_rows($check)==0){
$result=mysqli_query($con,"INSERT INTO admin_sch
	 values(
		'".$set['adm_cusr_uname']."',
		'".md5($set['adm_cusr_pwd'])."',
		'".$set['adm_cusr_type']."',
		'".$set['adm_cusr_name']."',
		'".$set['adm_cusr_pnum']."'
	);"
	);
	$arr['status']="suceed";
	echo json_encode($arr);
	exit;
}
else{
	$arr['status']="failed";
	echo json_encode($arr);
	exit;

}
?> 