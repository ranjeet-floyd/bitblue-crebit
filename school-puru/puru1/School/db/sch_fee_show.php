<?php
include('../../db.php');
parse_str($_POST['fee_show'], $grn);
$id=mysqli_fetch_assoc(mysqli_query($con,"SELECT * from user_sch	
	where Gr_num='".$grn['sch_grn']."'
	")
);
if(!$id){
	?>
	<p>No Matches Found of this GR/SR number</p>
	<?php
	exit;
}
$RCP= generateRandomString($con);
		
		
?>

<div class="main-container">
	<div class="Post-header ">
		<span>Fee Details</span>
	</div>
	<div class="Post-header hidden-name" >
			
		<img src="<?php $header_query=mysqli_query($con,'SELECT name,logo from info where Key_p="007"');
		$header_name=mysqli_fetch_row($header_query);
		echo('../Admin/school/'.$header_name[1]);?>">
	<span><?php 
		echo $header_name[0];
		?>
	
		</span>
		
	</div>
	<div class="post-content">
		<div class="post-text">
				<div class="box-left">
					Name:<span class="form-text"><b><?php echo $id['Name'];?></b></span>
				</div>
				<div class="box-right" >
					 Enroll No:: <span class="fee_chng_grn form-text"><b><?php echo $id['Gr_num'];?></b></span>
				</div>
				<div class="box-left">
					Medium:<span class="fee_chng_mdm form-text"> <b><?php echo $id['Medium'];?></b></span>
				</div>
				<div class="box-right">
					Standard: <span class="fee_chng_std form-text"><b><?php echo $id['Std'];?></b><span>
				</div>
				<div class="box-left">
					Section: <span class="form-text"><b><?php echo $id['Section'];?></b></span>
				</div>
				<form class="fee_chng_form" name="fee-change"  method="POST">
					<div class="box-right">
						Fee type: <select name="fee_chng_form_fee_type" onchange="fee_type()">
								<option value="default">Select One</option>
								<option value="Admission Fee">Admission</option>
								 <option value="Monthly Fee">Monthly</option>
								 <option value="Mid term 1 fee">Mid term 1</option>
								 <option value="Mid term 2 fee">Mid term 2</option>
								 <option value="End Term Fee">End Term</option>
								 <option value="Sports">Sports</option>
								 <option value="Computer Lab">Computer Lab</option>
								 <option value="Practical">Practical</option>
								 <option value="Insurance">Insurance</option>
								 <option value="Summer Camp">Summer Camp</option>
						 </select>
						
					</div>
					<div class="box-left" style="height:140px">
							Reciept Number: <input style="width:80px"type="text" value="<?php echo($RCP); ?>" name="fee_chng_form_reciept">
					</div>
					<div class="box-right" >
						Month <span class="fee_chng_form_hide form-text" style="height:140px;width:170px;float:right;display:inline-block;overflow:auto;position:relative;left:10px;"></span>
						
					</div>
					<div class="box-left">
							Payment mode:<select name="fee_chng_form_paym" onchange="cheque()">
							<option value="default">Select One</option>
							<option value="cash">Cash</option>
							<option value="cheque">Cheque</option>
						 </select>
					</div>					
					<div class="box-right">
						Cheque Number:<input style="width:60px;visibility:hidden" type='text' value="0" name="fee_chng_form_chq">
					</div>
					<div class="box-left">
						Amount:<b><span class="form-text fee_chng_form_hide_1"></span></b>
						<b><span class="form-text fee_chng_form_hide_4" style="display:none;"></span></b>
					</div>
					<div class="box-right">
						Late Fees:<b><span class="form-text fee_chng_form_hide_2" style="display:none;"></span></b>
						<b><span class="form-text fee_chng_form_hide_3" style="display:none;"></span></b>
					</div>
					
					<div class="box-right  ">
						<button class="sch_fee_verify"><span>Add fees</span></button>
					</div>
				</form>
		</div>
		</div>
		 </div>
		 <div class="main-container ">
			<div class="post-header">
				<span>Fee Available for this class</span>
			</div>
			<div class="post-content">
				<div class="post-text">
					<ul class="table-view">
					<li class="table-view-header" style="width:100px;">
						Fee type
					</li>
					<li class="table-view-header" style="width:100px;">
						Fee
					</li>
					<li class="table-view-header" style="width:100px;">
						Late fee 
					</li >
					<li class="table-view-header" style="width:100px;">
						One time 
					</li >
				</ul>
				<?php

					$query=mysqli_query($con,"select fee_type,fee,lfee,one_time from sch_cls_fee where Medium='".$id['Medium']."' AND Std='".$id['Std']."'");
					$i=0;
					if(mysqli_num_rows($query)!=0){
					while($result=mysqli_fetch_row($query)){
					$i++;
				?>	
				<ul class="table-view">
					<li style="width:100;">
						<?php echo $result[0]; ?>
					</li>
					<li  style="width:100px;">
						<?php echo $result[1]; ?>
					</li>
					<li style="width:100px;">
						<?php echo $result[2]; ?>
					</li>
					<li style="width:100px;">
						<?php echo $result[3]; ?>
					</li>

				</ul>
				<?php
					}
					}
					else{
					?>
					No fee added for this class
					<?php
					}
				?>

				</div>
			</div>
		 </div>
		 	<div class="main-container ">
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
				$query=mysqli_query($con,"select fee_type,fee,lfee,one_time from sch_cls_fee where Medium='".$id['Medium']."' AND Std='".$id['Std']."'");
				if(mysqli_num_rows($query)!=0){
					while($fees=mysqli_fetch_row($query)){
						$tran_query=mysqli_query($con,"select * from sch_tran where Gr_num='".$id['Gr_num']."' And fee_type='".$fees[0]."'");
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
								$add=array();
								$mon=array('Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec','Term 1','Term 2');
								$mon_query=mysqli_query($con,"select month from sch_tran  where Gr_num='".$id['Gr_num']."' And fee_type='".$fees[0]."'");
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
				function generateRandomString($con) {
						$length =5;
						$characters = '012345678901234567899874563210';
						$randomString = '';
						for ($i = 0; $i < $length; $i++) {
							$randomString .= $characters[rand(0, strlen($characters) - 1)];
						}
						$RCP='RC'.$randomString;
						$result=mysqli_query($con,"select * from sch_tran where Reciept='$RCP'");
					//	var_dump($result);
						if((mysqli_num_rows($result))!=null){
							generateRandomString($con);
						}
						return $RCP;
					} 	
			?>
			</div>
		</div>
	</div>


