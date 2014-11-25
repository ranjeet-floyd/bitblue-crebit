<?php
include('../../db.php');
$updc=$_POST['updc'];
$nclass=$_POST['nclass'];
$nmdm=$_POST['nmdm'];
$ncor=$_POST['ncor'];
$nsec=$_POST['nsec'];
//echo $ncor;
$result=mysqli_query($con,"SELECT * from clg_class where medium='".$nmdm."' AND course='".$ncor."'AND std='".$nclass."'");
//echo mysqli_num_rows($result);
if(mysqli_num_rows($result)!=0){
foreach($updc as $val){
	//print_r($val);
	$q=mysqli_query($con,"select Medium,Std, course from user_clg where Gr_num='".$val."'");
	$f=mysqli_fetch_row($q);
	//print_r($f);
	$query="Update user_clg  
		 SET	Std='".$nclass."', 
		 Medium='".$nmdm."',   
		 course='".$ncor."' , 
		 Section='".$nsec."'
		 where Gr_num='".$val."'";
		if(!($nmdm==$f[0] && $nclass==$f[1] && $ncor==$f[2]))
		mysqli_query($con,"Delete from clg_tran where Gr_num='".$val."' ");
	//	echo("Delete from sch_tran where Gr_num='".$val."' ");
		mysqli_query($con,$query);
	
	//echo $query;
	}
	$str['status']='suceed';
}
else{
		$str['status']='No Class to Matching Criteria available, please create class';
	}
	echo json_encode($str);
	exit;
?>