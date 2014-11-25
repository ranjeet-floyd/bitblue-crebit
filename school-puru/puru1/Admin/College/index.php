<?php
		include('../attach/header_clg.php');
		$query=mysqli_query($con,"select fee,one_time,medium,std from clg_cls_fee");
		$i=0;
		$rev=0;
		$late=0;
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
		$income=0;
		$query=mysqli_query($con,"select amount,late_fee,lflag from clg_tran");
		$i=0;
		while($jd=mysqli_fetch_row($query)){
				$income=$income+$jd[0];
			if($jd[2]=='no'){
				$late=$late+$jd[1];
			}	
		}
		$query1=mysqli_query($con,"select amount,late_fee from clg_tran where date='".date("Y-m-d")."'");
		$today=0;
		while($tod=mysqli_fetch_row($query1)){
			$today=$today+$tod[0]+$tod[1];
		}
		$get=($income+$late);
?>
	<div class="span-right">
		<div class="main-container">
			 <div class="post-header">
				<span> Home</span>
			 </div>
			 <div class="post-content">
				<div class="post-text">
					<ul class="home-menu">
						<a class="home-menu-link" href="revenue.php">
							<li class="home-menu-li">
								<span>
									<?php echo ("Revenue = ".$get )?>
								</span>
							</li>
						</a>
						<a class="home-menu-link" href="today.php">
							<li class="home-menu-li">
								<span>
										<?php echo ("Todays = ".$today)?>
								</span>
							</li>
						</a>
					</ul>
				</div>
			 </div>
		</div>
	
	</div>
		<?php
		include('../attach/footer_clg.php');
	
?>