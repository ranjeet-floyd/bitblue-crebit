<?php
include('../../db.php');
$mdm=$_POST['mdm'];
$std=$_POST['std'];
$cor=$_POST['cor'];
$query=mysqli_query($con,"select no_of_div from clg_class where Medium='$mdm' AND Std='$std' AND course='$cor'");
$result=mysqli_fetch_row($query);
//print_r($count);
foreach($result as $key=>$val){
	$count=$val;
}
//echo $count;
$arr1=array('A','B','C','D','E','F','G','H','I','J','K','L');
$arr['data']=array_slice($arr1,0,$count);
echo json_encode($arr);
exit;
?>