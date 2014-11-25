<?php
include('../../db.php');
$result=mysqli_fetch_row(mysqli_query($con,"Select fee,lfee,one_time from clg_cls_fee where Medium='".trim($_POST['fee_chng_mdm'])."' AND Std='".trim($_POST['fee_chng_std'])."' And fee_type='".trim($_POST['fee_chng_type'])."'"));
if($result){
	//print_r($result[2]);
	if($result[2]=='Per Year'){
		$arr['data']='<select name="fee_chng_form_month"><option val="one time">One time</select><br/>Late Paid<select name="fee_chng_form_ot">
							<option value="yes">No</option>
							<option value="no">Yes</option>
							
						</select>';
		$arr['fee']=$result[0];
		$arr['lfee']=$result[1];
		$arr['status']="suceed";
		//$arr['late']='';
		echo json_encode($arr);
		exit;
	}
	else{
		$mon=array('Jul','Aug','Sep','Oct','Nov','Dec','Jan','Feb','Mar','Apr','May','Jun');
		$q1=mysqli_query($con,"select Month from clg_tran where Gr_num='".$_POST['fee_chng_grn']."' AND fee_type='".$_POST['fee_chng_type']."'");
		//echo("select Month from sch_tran where Gr_num='".$_POST['fee_chng_grn']."' AND fee_type='".$_POST['fee_chng_type']."'");
		$month=array();
		$i=0;
		while($k=mysqli_fetch_row($q1)){
			$month[$i++]=$k[0];
		}
		//print_R($month);
		$diff=array();
		$diff=array_diff($mon,$month);
		$a="<div style='width:150px;height:20px;display:inline;float:right'>";
		$a=$a."<span style='text-indent:110px;display:inline-block'>Late</span><br/></div>";
		foreach($diff as $val){
			$a=$a."<div style='width:150px;height:20px;display:inline;float:right'><span class='sel_mon_span'><input class='sel_mon' type='checkbox' value='".$val."' name='fee_chng_form_month[]'></span>".$val."";
			$a=$a."<span class='sel_late_span'><input class='sel_late' type='checkbox' name='fee_chng_form_ot[]' value='".$val."' ></span><br/></div>";
		}
		$arr['data']=$a;
		//$arr['late']=$b."</div>";
		$arr['status']="suceed";
		$arr['fee']=$result[0];
		$arr['lfee']=$result[1];
		echo json_encode($arr);
		exit;					
	}

}
else{
	$arr['result']="failed";
	echo json_encode($arr);
	exit;
}
?>

