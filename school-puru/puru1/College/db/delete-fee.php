<?php

	include('../../db.php');
	mysqli_query($con,"DELETE FROM clg_tran where Gr_num='".trim($_POST['fee_grn'])."' AND fee_type='".trim($_POST['fee_typ'])."' AND reciept='".trim($_POST['fee_rec'])."'  AND month='".trim($_POST['fee_mon'])."'");
	//echo json_encode("DELETE FROM clg_tran where Gr_num='".trim($_POST['fee_grn'])."' AND fee_type='".trim($_POST['fee_typ'])."'");
?>