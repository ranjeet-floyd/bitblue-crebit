 <?php
	include('../../db.php');
		$fee=0;
		$late=0;
		$str=$_POST['strdate'];
		$end=$_POST['enddate'];
		$query=mysqli_query($con,"select * from clg_tran where date between '$str' and '$end' order by date DESC");
		// echo("select * from sch_tran where date between '$str'and '$end' order by date DESC");
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
						<li class="table-view-header" style="width:50px;">
							Name
						</li>
						<li class="table-view-header" style="width:60px;">
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
						<li style="width:60px;">
							<?php echo $result[0] ; ?>
						</li>
						<li  style="width:50px;">
							<?php echo $result[2];$fee=$fee+$result[2]; ?>
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
							<?php echo $result[9]; $late=$late+$result[9];?>
						</li>
						<li style="width:100px;">
							<?php echo $result[10]; ?>
						</li>
					</ul>
			
	<?php
	}
	}
	else{
		echo('No details exists for this duration');
	}
?>
				<ul class="table-view">
						<li class="table-view-header" style="width:150px;">
							 <?php echo("Amount:-".$fee);?>
						</li>
						<li class="table-view-header" style="width:150px;">
							 <?php echo("Late fee:-".$late);?> 
						</li>
						<li class="table-view-header" style="width:150px;">
							 <?php echo("Total Amount:-".($fee+$late));?>
						</li>		
					</ul>