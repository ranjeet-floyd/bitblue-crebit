<?php
	include('../attach/header_clg.php');
	//echo(date("Y-m-d"));
	$query=mysqli_query($con,"select * from clg_tran where date='".date("Y-m-d")."'");
	$i=0;
	
		?>
		<div class="span-right">
		<div class="main-container">
		<div class="post-header">
			<span>Recieved Revenue</span>
		</div>
		<?php
		if(mysqli_num_rows($query)!=0){
		?>
		<div class="post-content">
			<div class="post-text">
					<ul class="table-view">
						<li class="table-view-header" style="width:50px;">
							Name
						</li>
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
							$name=mysqli_fetch_row(mysqli_query($con,"select Name from user_clg where Gr_num='".$result[1]."'"));
							$i++; 
					?>
					<ul class="table-view ">
						<li style="width:50px;">
							<?php echo $name[0] ; ?>
						</li>
						<li style="width:50px;">
							<?php echo $result[0] ; ?>
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
	?>
		</div></div>
		
	<?php
	}
	else{
	?>
	<div class="post-content">
	No fee history for today
	</div>
	<?php
	}
?>	
	</div></div>	
		
	
	<?php
	include('../attach/footer_clg.php');
?>