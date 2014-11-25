<?php
include('../../db.php');
parse_str($_POST['adm_set'], $set);
$id=mysqli_fetch_assoc(mysqli_query($con,"SELECT * from clg_class	
	where Medium='".$set['adm_set_mdm']."' AND
		Std='".$set['adm_set_std']."' AND
		Course='".$set['adm_set_cor']."'
	")
);

if($id==null){
	?>
	0
	<?php
	exit;
}

$result=mysqli_query($con,"INSERT INTO clg_cls_fee
	 values(
		'".trim($set['adm_set_mdm'])."',
		'".trim($set['adm_set_cor'])."',
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
	<ul class="table-view ">
	<li style="width:65px;">
		<?php  echo ++$_POST['table_view_id']; ?>
	</li>
	<li style="width:65px;">
		<?php echo $set['adm_set_mdm']; ?>
	</li>
	<li style="width:65px;">
		<?php echo $set['adm_set_cor']; ?>
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