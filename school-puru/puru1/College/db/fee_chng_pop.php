<?php
include('../../db.php');
parse_str($_POST['fee_chng'], $fee);
//print_r($fee['fee_chng_form_ot']);
$i=0;
$late_fee=0;
if($fee['fee_chng_form_month']!='One time' ){
	foreach($fee['fee_chng_form_month'] as $val){
	//print($val);
		if(isset($fee['fee_chng_form_ot'])){
			foreach($fee['fee_chng_form_ot'] as $key){
				if($key==$val){
					$late_fee=($_POST['fee_chng_lfee']);
					$lflag='no';
					Goto A;
				}
				else{
					$late_fee=0;
					$lflag='yes';
				}
			}
			A:
		}
		else{
			$late_fee=0;
			$lflag='yes';
		}
		//print_r($fee['fee_chng_form_month'][$i]);
		$check=mysqli_num_rows(mysqli_query($con,"select * from clg_tran where Gr_num='".$_POST['fee_chng_grn']."' AND Fee_type='".$fee['fee_chng_form_fee_type']."' AND Month='".$fee['fee_chng_form_month'][$i]."'"));
		if($check==0){
			$result=mysqli_query($con,"INSERT into clg_tran values(
					'".trim($fee['fee_chng_form_reciept'])."',
					'".trim($_POST['fee_chng_grn'])."',
					'".trim($_POST['fee_chng_amount'])."',
					'".trim($val)."',
					'".date("Y")."',
					'".trim($fee['fee_chng_form_fee_type'])."',
					'".trim($fee['fee_chng_form_paym'])."',
					'".trim($fee['fee_chng_form_chq'])."',
					'".$lflag."',
					'".$late_fee."',
					'".date("Y-m-d")."'
					)
			");
			$i++;
		}
		else{
			$arr['status']='failed';
			echo json_encode($arr);
			exit;
		}
	}
}
else{
	//echo($fee['fee_chng_form_ot']);
	if($fee['fee_chng_form_ot']=='no'){
		$late_fee=($_POST['fee_chng_lfee']);
	}
	else{
		$late_fee=0;
	}
	$check=mysqli_num_rows(mysqli_query($con,"select * from clg_tran where Gr_num='".$_POST['fee_chng_grn']."' AND Fee_type='".$fee['fee_chng_form_fee_type']."'"));
	if($check==0){
		$result=mysqli_query($con,"INSERT into clg_tran values(
				'".trim($fee['fee_chng_form_reciept'])."',
				'".trim($_POST['fee_chng_grn'])."',
				'".trim($_POST['fee_chng_amount'])."',
				'".trim($fee['fee_chng_form_month'])."',
				'".date("Y")."',
				'".trim($fee['fee_chng_form_fee_type'])."',
				'".trim($fee['fee_chng_form_paym'])."',
				'".trim($fee['fee_chng_form_chq'])."',
				'".$fee['fee_chng_form_ot']."',
				'".$late_fee."',
				'".date("Y-m-d")."'
				)
		");
	}
	else{
		$arr['status']='failed';
		echo json_encode($arr);
		exit;
	}
}

	$details=mysqli_fetch_row(mysqli_query($con,"select Name,Medium,Std,course from user_clg where Gr_num='".trim($_POST['fee_chng_grn'])."'"));
	$arr['det']=$details;
	$arr['rec']=trim($fee['fee_chng_form_reciept']);
	$arr['month']=$fee['fee_chng_form_month'];
	$arr['chq']=trim($fee['fee_chng_form_chq']);
	$arr['Gr']=trim($_POST['fee_chng_grn']);
	$arr['lfee']=trim($_POST['fee_chng_lfee']);
	$arr['pay_mode']=trim($fee['fee_chng_form_paym']);
	$arr['amount']=trim($_POST['fee_chng_amount']);
	$arr['typ']=$fee['fee_chng_form_fee_type'];
	if(isset($fee['fee_chng_form_ot'])){
		$arr['ot']=$fee['fee_chng_form_ot'];
	}
	else{
		$arr['ot']='No';
	}
	$arr['status']='suceed';
	echo json_encode($arr);
	exit;
	
?>