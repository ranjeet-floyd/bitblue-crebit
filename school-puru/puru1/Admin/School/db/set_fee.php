<?php
include('../../db.php');
	if($con){
		//echo("otehrde");
	}
//$arr=array();
parse_str($_POST['adm_set'], $set);
$id=mysqli_fetch_assoc(mysqli_query($con,"SELECT * from sch_class	
	where Medium='".$set['adm_set_mdm']."' AND
		Std='".$set['adm_set_std']."'
	")
);

if($id==null){
	?>
	0
	<?php
	exit;
}

$result=mysqli_query($con,"INSERT INTO sch_cls_fee
	 values(
		'".trim($set['adm_set_mdm'])."',
		'".trim($set['adm_set_std'])."',
		'".trim($set['adm_set_fee_type'])."',
		'".trim($set['adm_set_fee'])."',
		'".trim($set['adm_set_lfee'])."',
		'".trim($set['adm_set_one'])."'
	);"
	);
	if($result==null){
	?>
	1
	<?php
	exit;
	}?>
	<ul class="table-view <?php echo 'view-'.$_POST['table_view_id'] ;?> ">
	<li style="width:65px;">
		<?php  echo $_POST['table_view_id']; ?>
	</li >
	<li style="width:65px;">
		<?php echo $set['adm_set_mdm']; ?>
	</li>
	<li style="width:65px;">
		<?php echo $set['adm_set_std']; ?>
	</li>
	<li   style="width:100px;">
		<?php echo $set['adm_set_fee_type'] ?>
	</li>
	<li style="width:65px;" >
		<?php echo $set['adm_set_fee'] ?>
	</li>
	<li style="width:65px;">
		<?php echo $set['adm_set_lfee'] ?>
	</li>
	<li style="width:65px;">
		<?php echo $set['adm_set_one'] ?>
	</li>
	<li style="width:65px;">
		0
	</li>
</ul>
	<?php

exit;
?> 