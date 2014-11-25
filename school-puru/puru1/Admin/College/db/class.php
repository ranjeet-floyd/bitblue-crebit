<?php
parse_str($_POST['adm_clg_cls'], $class);
//echo(Si)
include('../../db.php');
$check=mysqli_query($con,"select * from clg_class where Medium='".trim($class['adm_clg_cls_mdm'])."' AND Std='".trim($class['adm_clg_cls_std'])."' AND Course='".$class['adm_clg_cls_cor']."'");
if(mysqli_num_rows($check)==0){
	mysqli_query($con,"Insert into clg_class 
				values(
				'".trim($class['adm_clg_cls_mdm'])."',
				'".trim($class['adm_clg_cls_cor'])."',
				'".trim($class['adm_clg_cls_std'])."',
				'".trim($class['adm_clg_cls_div'])."',
				'".date("Y-m-d")."'
				)
			");		

	$query=mysqli_query($con,"select * from clg_class
							where clg_class.Medium='".trim($class['adm_clg_cls_mdm'])."' 
							AND clg_class.Std='".trim($class['adm_clg_cls_std'])."'
							AND clg_class.course='".trim($class['adm_clg_cls_cor'])."';
	");
	
	while($result=mysqli_fetch_row($query)){
			
				?>
				<ul class="table-view"	 >
				
					<li style="width:100px"  >
						<?php echo $result[0]; ?>
					</li>
					<li  style="width:100px;">
						<?php echo $result[1]; ?>
					</li>
					<li  style="width:100px;">
						<?php echo $result[2]; ?>
					</li>
					<li style="width:100px">
						<?php echo $result[3]; ?>
					</li>
					<li style="width:100px;">
						<?php
							$count=mysqli_num_rows(mysqli_query($con,"select * from user_clg where Medium='".$result[0]."' AND std='".$result[2]."' AND course='".$result[1]."'"));
							echo $count;
						?>
					</li>	
					
					
				</ul>
				<?php
					}

	exit;
}
else{
	exit;
}
?> 