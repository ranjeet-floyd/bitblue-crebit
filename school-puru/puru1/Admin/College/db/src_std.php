<?php
	include('../../db.php');
	if($_POST['count']=='1'){
		parse_str($_POST['adm_src'], $grn);
		$id=mysqli_fetch_assoc(mysqli_query($con,"SELECT * from user_clg	
			where Gr_num='".$grn['adm_sch_src_grn']."'
		")
		);
		$query=mysqli_query($con,"select * from clg_tran where Gr_num ='".$grn['adm_sch_src_grn']."'");
	}
	else{
		$id=mysqli_fetch_assoc(mysqli_query($con,"SELECT * from user_clg	
			where Gr_num='".$_POST['adm_src']."'
		")
		);
			$query=mysqli_query($con,"select * from clg_tran where Gr_num ='".$_POST['adm_src']."'");
	}
	if(!$id){
		?>
		invalid GR number
		<?php
		exit;
	}
?>
	<div class="main-container">
		<div class="Post-header">
			<span>Personal Details</span>
		</div>
		<div class="post-content">
			<div class="post-text">
				<div class="box-left">
					Name:<span class="form-text"><b><?php echo $id['Name'];?></b></span>
				</div>
				<div class="box-right" >
					Gr/Sr Number: <span class="form-text"><b><?php echo $id['Gr_num'];?></b></span>
				</div>
				<div class="box-left">
					Medium:<span class="form-text"> <b><?php echo $id['Medium'];?></b></span>
				</div>
				<div class="box-right">
					Standard: <span class="form-text"><b><?php echo $id['Std'];?></b><span>
				</div>
				<div class="box-left">
					Section: <span class="form-text"><b><?php echo $id['Section'];?></b></span>
				</div>
			</div>
		</div>
	</div>
	<div class="main-container">
	<div class="Post-header">
			<span>Fee history</span>
		</div>
		<div class="post-content">
			<div class="post-text">
<?php
	$i=0;
	if(mysqli_num_rows($query)!=0){
?>
	
				<ul class="table-view">
					<li class="table-view-header" style="width:50px;">
						Reciept
					</li>
					<li class="table-view-header" style="width:50px;">
						Amount
					</li>
					<li class="table-view-header" style="width:50px;">
						Month
					</li >
					<li class="table-view-header" style="width:110px;">
						Fee type
					</li>
					<li class="table-view-header" style="width:50px;">
						Pay mode
					</li>
					<li class="table-view-header" style="width:50px;">
						Cheque No
					</li>
					<li class="table-view-header" style="width:50px;">
						Paid on time 
					</li>
					<li class="table-view-header" style="width:50px;">
						Late fee 
					</li>
					<li class="table-view-header" style="width:100px;">
						Date
					</li>		
				</ul>
				<?php
						while($result=mysqli_fetch_row($query)){
						$i++; 
				?>
				<ul class="table-view ">
					<li style="width:50px;">
						<?php echo $result[0]; ?>
					</li>
					<li  style="width:50px;">
						<?php echo $result[2]; ?>
					</li>
					<li style="width:50px;">
						<?php echo $result[3]; ?>
					</li>
					<li style="width:110px;">
						<?php echo $result[5]; ?>
					</li>
					<li style="width:50px;">
						<?php echo $result[6]; ?>
					</li>
					<li style="width:50px;">
						<?php echo $result[7]; ?>
					</li>
					<li style="width:50px;">
						<?php echo $result[8]; ?>
					</li>
					<li style="width:50px;">
						<?php echo $result[9]; ?>
					</li>
					<li style="width:100px;">
						<?php echo $result[10]; ?>
					</li>
				</ul>
				<?php
				}

	}
	else{
	?>
	No fee history for this student.
	<?php
	}
?>
	</div>
		</div>
	</div>
	<div class="main-container">
		<div class="Post-header">
			<span>Unpaid fees</span>
		</div>
		<div class="post-content">
			<div class="post-text">
				<ul class="table-view">
					<li class="table-view-header" style="width:100px;">
						Fee type
					</li>
					<li class="table-view-header" style="width:100px;">
						Amount
					</li>
					<li class="table-view-header" style="width:100px;">
						Frequency
					</li >
					<li class="table-view-header" style="width:100px;">
						Late fees
					</li>
					<li class="table-view-header" style="width:250px;">
						Remaining Month(if per month)
					</li>
				</ul>
			<?php
				$query=mysqli_query($con,"select fee_type,fee,lfee,one_time from clg_cls_fee where Medium='".$id['Medium']."' AND course='".$id['course']."' AND Std='".$id['Std']."'");
				if(mysqli_num_rows($query)!=0){
					while($fees=mysqli_fetch_row($query)){
						$tran_query=mysqli_query($con,"select * from clg_tran where Gr_num='".$id['Gr_num']."' And fee_type='".$fees[0]."'");
						if(mysqli_num_rows($tran_query)==0&&$fees[3]=='Per Year'){
							$tran=mysqli_fetch_row($tran_query);
						?>
							<ul class="table-view">
								<li style="width:100px;">
									<?php echo $fees[0];?>
								</li>
								<li  style="width:100px;">
									<?php echo $fees[1];?>
								</li>
								<li style="width:100px;">
									<?php echo $fees[3];?>
								</li >
								<li  style="width:100px;">
									<?php echo $fees[2];?>
								</li>
								<li style="width:250px">
									NA
								</li>
							</ul>
					<?php
						}
						if($fees[3]=='Per Month'){
								$j=0;
								$mon=array('Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec');
								$mon_query=mysqli_query($con,"select month from clg_tran  where Gr_num='".$id['Gr_num']."' And fee_type='".$fees[0]."'");
								$add=array();
								while($mon_add=mysqli_fetch_array($mon_query)){
									$add[$j]=$mon_add[0];
									$j++;
								}
							//	print_r($add);
								//echo("select month from sch_tran  where Gr_num='".$id['Gr_num']."' And fee_type='".$fees[0]."'");
								?>
								<ul class="table-view">
									<li style="width:100px;">
										<?php echo $fees[0];?>
									</li>
									<li  style="width:100px;">
										<?php echo $fees[1];?>
									</li>
									<li style="width:100px;">
										<?php echo $fees[3];?>
									</li >
									<li  style="width:100px;">
										<?php echo $fees[2];?>
									</li>
									<li  style="display:block;width:250px;">
										<?php 
										foreach(array_diff($mon,$add) as $key){
											echo $key;
										}
										?>
									</li>
									</ul>
								<?php
						}
					}
				}
				else{
					?>
					No added fee available for this class, ask admin to add fee
				<?php 
				}
			?>
			</div>
		</div>
	</div>
	<?php 
	
	exit;
	?>