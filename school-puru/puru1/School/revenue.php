<?php
	include('../attach/header_sch.php');
	$rev=0;
	$income=0;
	$late=0;
	$pending=0;
	$query=mysqli_query($con,"select fee,one_time,medium,std from sch_cls_fee");
	$i=0;
	/*while($id=mysqli_fetch_row($query)){
		$count=mysqli_num_rows(mysqli_query($con,"select * from user_sch where medium='".$id[2]."' AND std='".$id[3]."'"));
	//	echo $count;
		if($id[1]=='Per Year'){
				$rev=($id[0]*$count+$rev);
			}
			else{
				$rev=(($id[0]*12)*$count)+$rev;
			}
	}*/
	$query=mysqli_query($con,"select amount,late_fee,lflag from sch_tran");
	$i=0;
	while($jd=mysqli_fetch_row($query)){
			$income=$income+$jd[0];
		if($jd[2]=='no'){
			$late=$late+$jd[1];
		}	
	}


?>
<div class="span-right">
	<div class="main-container">
		<div class="post-header">
			<span> Revenue</span>
		</div>
	    <div class="post-content">
			<div class="post-text">
				<div class="rev-div rev-recieve">
					<div class="rev-div-header">
						Total Recieved Revenue	
					</div>
					<div class="rev-content">
						<div class="content-block">
							<div class="block-top">
								From Direct Fee
							</div>
							<div class="block-bottom">
								<?php echo $income ?>
							</div>
						</div>
						<div class="content-block">
							<div class="block-top">
								From late Fee
							</div>
							<div class="block-bottom">
								<?php echo $late ?>
							</div>
						</div>
					</div>
				</div>
				<div class="rev-div rev-pend" style="cursor:pointer">
					<div class="rev-div-header">
						Total Pending
					</div>
					<div class="rev-content">

					</div>
				</div>
			</div>
		</div>
	</div>
	<div class="rev-div-main">
	</div>
</div>
	<?php
	include('../attach/footer_sch.php');
?>