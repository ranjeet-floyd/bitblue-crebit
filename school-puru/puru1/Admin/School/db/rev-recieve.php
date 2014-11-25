<?php
	include('../../db.php');
	//Added By Ranjeet || Filter values
	$drpMedium=$_POST['drpMedium'];
	$drpStd=$_POST['drpStd'];
	$drpSec=$_POST['drpSec'];
	$query=mysqli_query($con,"select * from sch_tran order by date DESC");
	$i=0;
	if(mysqli_num_rows($query)!=0){					
	?>

		<div class="main-container">
		<div class="post-header">
			<span>Recieved Revenue</span>
			
	
	
		</div>
		<div class="post-content">
			<div class="post-text">
					<ul class="table-view">
						
						<li class="table-view-header" >
							Reciept
						</li>
						<li class="table-view-header" >
							Name
						</li><li class="table-view-header" >
							Medium
						</li><li class="table-view-header" >
							Std
						</li><li class="table-view-header" >
							Sec
						</li>
						<li class="table-view-header" >
							Amo
						</li>
						<li class="table-view-header" >
							Mon
						</li >
						<li class="table-view-header" >
							FeeType
						</li>
						<li class="table-view-header" >
							Pay
						</li>
						<li class="table-view-header" >
							Cheque#
						</li>
						<li class="table-view-header" >
							Gr No 
						 </li>
						<li class="table-view-header" >
						LateFee
						
							 
						</li>
						<li class="table-view-header" >
							Date
						</li>		
					</ul>
					<?php
							while($result=mysqli_fetch_row($query)){
							if($drpMedium != -1 )				
							$name=mysqli_fetch_row(mysqli_query($con,"select Name,Medium,Std,Section ,Gr_num from user_sch where Gr_num='".$result[1]."' AND Medium='".$drpMedium."' AND Std='".$drpStd."' AND Section='".$drpSec."' "));
						else				
							$name=mysqli_fetch_row(mysqli_query($con,"select Name,Medium,Std,Section,Gr_num from user_sch where Gr_num='".$result[1]."'"));
							$i++; 
					if($name[0] !=""){		
					?>
					<ul class="table-view ">
						
						<li>
							<?php echo $result[0] ; ?>
						</li>
						<li >
							<?php echo $name[0] ; ?>
						</li>
						<li >
							<?php echo $name[1] ; ?>
						</li>
						<li >
							<?php echo $name[2] ; ?>
						</li>
						<li >
							<?php echo $name[3] ; ?>
						</li>
						<li  >
							<?php echo $result[2]; ?>
						</li>
						<li >
							<?php echo $result[3]; ?>
						</li>
						<li >
							<?php echo $result[5]; ?>
						</li>
						<li >
							<?php echo $result[6]; ?>
						</li>
						<li >
							<?php echo $result[7]; ?>
						</li>
						<li >
							<?php echo $name[4]; ?>
						</li>
						<li >
							<?php echo $result[9]; ?>
						</li>
						<li >
							<?php echo $result[10]; ?>
						</li>
					</ul>
	<?php
	}
   ?>
			
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