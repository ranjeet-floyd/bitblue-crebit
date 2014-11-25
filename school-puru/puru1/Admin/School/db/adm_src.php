<?php
include('../../db.php');
parse_str($_POST['adm_src'], $set);
$id=mysqli_fetch_assoc(mysqli_query($con,"SELECT Enroll from user_sch	
	where Gr_num='".$set['adm_src_grn']."'
	")
);
//print_r($id);
$arr['data']=$id;
if(!$id){
	$arr['status']='Failed';
	echo json_encode($arr);
	exit;
}
$arr['status']='suceed';
echo json_encode($arr);
exit;
?>