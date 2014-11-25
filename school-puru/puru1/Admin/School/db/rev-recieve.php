<?php
	include('../../db.php');
	//Added By Ranjeet || Filter values
	$drpMedium=$_POST['drpMedium'];
	$drpStd=$_POST['drpStd'];
	$drpSec=$_POST['drpSec'];
	$selectedMonths = $_POST['selectedMonths'];//Added | Ranjeet | 24-Nov
	$query=mysqli_query($con,"select * from sch_tran where Month in  (".$selectedMonths.") order by date DESC;");
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
						<li class="table-view-header" style="width:200px;" >
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
						<li class="table-view-header" style="width:100px">
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
						<li class="table-view-header" style="width:100px">
							Date
						</li>		
					</ul>
					<?php
							while($result=mysqli_fetch_row($query)){
							if($drpMedium != -1 )				
								//modified : Ranjeet || 24 Nov
							$name=mysqli_fetch_row(mysqli_query($con,"select user_sch.Name,user_sch.Medium,user_sch.Std,user_sch.Section ,user_sch.Gr_num from user_sch 
								INNER JOIN sch_tran ON user_sch.Gr_num = sch_tran.Gr_num
								and sch_tran.Month in  (".$selectedMonths.") 
								where user_sch.Gr_num='".$result[1]."' AND user_sch.Medium='".$drpMedium."' 
								AND user_sch.Std='".$drpStd."' AND user_sch.Section='".$drpSec."' "));
						else				
							$name=mysqli_fetch_row(mysqli_query($con,"select Name,Medium,Std,Section,Gr_num from user_sch where Gr_num='".$result[1]."'"));
							$i++; 
					if($name[0] !=""){		
					?>
					<ul class="table-view ">
						
						<li>
							<?php echo $result[0] ; ?>
						</li>
						<li  style="width:200px;">
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
						<li style="width:100px">
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
						<li style="width:100px">
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