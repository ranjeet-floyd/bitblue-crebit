<?php
include('../../db.php');
$yearly=0;
$monthly=0;
//Modified By Ranjeet || 23-Sep
$drpMedium=$_POST['drpMedium'];
$drpStd=$_POST['drpStd'];
$drpSec=$_POST['drpSec'];
$selectedMonths = $_POST['selectedMonths'];//Added | Ranjeet | 24-Nov
//$drpFMonth = $_POST['drpFMonth'];//Added | Ranjeet | 24-Nov
//print $drpFMonth ;
//print $drpTMonth;
//$monArray=array('Jul','Aug','Sep','Oct','Nov','Dec','Jan','Feb','Mar','Apr','May','Jun');
//$selectedMonths = "'Jul','Aug','Sep','Oct','Nov','Dec','Jan','Feb','Mar','Apr','May','Jun'";
//print $selectedMonths;


//if($drpFMonth != 0 || $drpTMonth != 0){
	
	//$selectedMonths = "";
	//$selectedMonths=array_slice($monArray,$drpFMonth-1,$drpTMonth-1);
//	for ($i=$drpFMonth; $i <= $drpTMonth; $i++) { 
//		$selectedMonths += "Test" + $monArray[i-1] + ',';
		//echo  $monArray[i-1];
//	}
//}
	


$condi = "";

$query=mysqli_query($con,"Select user_sch.Gr_num, sch_cls_fee.fee_type, user_sch.Name, sch_cls_fee.fee 
				,user_sch.Medium,user_sch.Std,user_sch.Section
				from user_sch
				Inner Join sch_cls_fee
				On user_sch.Medium=sch_cls_fee.Medium 
				And user_sch.Std=sch_cls_fee.Std 
				AND user_sch.Medium = '".$drpMedium."' AND user_sch.Std = '".$drpStd."' 
				AND user_sch.Section = '".$drpSec."' 
				INNER JOIN sch_tran ON user_sch.Gr_num = sch_tran.Gr_num
				and sch_tran.Month in  (".$selectedMonths.") 
				where sch_cls_fee.one_time ='Per Year'
				order by user_sch.Gr_num");
$query1=mysqli_query($con,"Select user_sch.Gr_num, sch_tran.fee_type ,user_sch.Name, sch_tran.amount
				,user_sch.Medium,user_sch.Std,user_sch.Section
				from user_sch
				Inner Join sch_tran
				On user_sch.Gr_num=sch_tran.Gr_num 
				AND user_sch.Medium = '".$drpMedium."' AND user_sch.Std = '".$drpStd."' 
				AND user_sch.Section = '".$drpSec."' 
				where sch_tran.Month in  (".$selectedMonths.")
				order by user_sch.Gr_num");


$i=0;
while($r1=mysqli_fetch_row($query)){
	$arr1[$i]=$r1;
	$i++;
}
$i=0;
while($r2=mysqli_fetch_row($query1)){
	$arr2[$i]=$r2;
	$i++;
}
$i=0;
if(!empty($arr1) && !empty($arr2))
foreach($arr1 as $k1=>$v1){
	foreach($arr2 as $k2=>$v2){
		if($v1[0]==$v2[0] && $v1[1]==$v2[1]){
			unset($arr1[$i]);
		}

	}
	$i++;
}
else if (empty($arr1)){
	echo('no fee has been added, try adding fees');
	exit;
}
//var_dump($arr1);
?>
	<div class="main-container">
		<div class="post-header">
			<span class="yearly">Pending Fees ( Yearly Fee )</span>
		</div>
		<div class="post-content">
			<div class="post-text">
				<ul class="table-view">
					<li class="table-view-header"  style="width:200px;">
						Name
					</li>
					<li class="table-view-header" style="width:100px;">
						Med
					</li>
					<li class="table-view-header" style="width:100px;">
						Std
					</li>
					<li class="table-view-header" style="width:100px;">
						Sec
					</li>
					<li class="table-view-header" style="width:100px;">
						GR Number
					</li>
					<li class="table-view-header" style="width:100px;">
						Amount
					</li>
					<li class="table-view-header" style="width:auto">
						Fee type
					</li>
				</ul>
			
		<?php
		foreach($arr1 as $key=>$val){
		?>
			<ul class="table-view">
					<li style="width:200px;">
						<?php echo $val[2];?>
					</li>
					<li style="width:100px;">
						<?php echo $val[4];?>
					</li>
					<li style="width:100px;">
						<?php echo $val[5];?>
					</li>
					<li style="width:100px;">
						<?php echo $val[6];?>
					</li>
					<li style="width:100px;">
						<?php echo $val[0];?>
					</li>
					<li style="width:100px;">
						<?php echo $val[3];
						$yearly=$yearly+$val[3];
						?>
					</li>
					<li  class="revenue-pend" style="width:auto;">
						<?php echo $val[1];?>
					</li>
				</ul>
				<?php
		}
		$i=0;
?>
<ul class="table-view">
					<li class="table-view-header" style="width:100px;">
						
					</li>
					<li class="table-view-header" style="width:100px;">
						Total Pending Fee
					</li>
					<li class="table-view-header" style="width:110px;">
						<?php echo $yearly;?>
					</li>
					<li class="table-view-header" style="width:100px;">
						
					</li>
				</ul>
	</div>
</div>
<div class="post-header">
			<span>Pending Fees ( Monthly Fee )</span>
		</div>
		<div class="post-content">
			<div class="post-text">
			<?php
				$query=mysqli_query($con,"Select user_sch.Gr_num, sch_cls_fee.fee_type, user_sch.Name, sch_cls_fee.fee
					,user_sch.Medium,user_sch.Std,user_sch.Section
				from user_sch
				Inner Join sch_cls_fee
				On user_sch.Medium=sch_cls_fee.Medium 
				And user_sch.Std=sch_cls_fee.Std 
				AND user_sch.Medium = '".$drpMedium."' AND user_sch.Std = '".$drpStd."' 
				AND user_sch.Section = '".$drpSec."' 
				INNER JOIN sch_tran ON user_sch.Gr_num = sch_tran.Gr_num
				and sch_tran.Month in  (".$selectedMonths.")
				where sch_cls_fee.one_time ='Per Month'
				order by user_sch.Gr_num	
		");
$query1=mysqli_query($con,"Select user_sch.Gr_num, sch_tran.fee_type ,user_sch.Name, sch_tran.amount, sch_tran.month
				,user_sch.Medium,user_sch.Std,user_sch.Section
				from user_sch
				Inner Join sch_tran
				On user_sch.Gr_num=sch_tran.Gr_num  
				AND user_sch.Medium = '".$drpMedium."' AND user_sch.Std = '".$drpStd."' 
				AND user_sch.Section = '".$drpSec."' 
				where sch_tran.month NOT In ('One time') AND sch_tran.Month in  (".$selectedMonths.") 
				order by user_sch.Gr_num
		");
$i=0;
while($r1=mysqli_fetch_row($query)){
	$arr3[$i]=$r1;
	$i++;
}
$i=0;
while($r2=mysqli_fetch_row($query1)){
	$arr4[$i]=$r2;
	$i++;
}
$i=0;
if(!empty($arr3) && !empty($arr4)){
foreach($arr3 as $k1=>$v1){
	foreach($arr4 as $k2=>$v2){
		if($v1[0]==$v2[0] && $v1[1]==$v2[1]){
			unset($arr3[$i]);
		}

	}
	$i++;
}
}
else if (empty($arr3)){
	echo('no fee has been added, try adding fees');
	exit;
}



			?>
				<ul class="table-view">
					<li class="table-view-header" style="width:200px;">
						Name
					</li>
					<li class="table-view-header" style="width:100px;">
						Med
					</li>
					<li class="table-view-header" style="width:100px;">
						Std
					</li>
					<li class="table-view-header" style="width:100px;">
						Sec
					</li>
					<li class="table-view-header" style="width:100px;">
						GR Number
					</li>
					<li class="table-view-header" style="width:100px;">
						Amount
					</li>
					<li class="table-view-header" style="width:110px;">
						Month
					</li>
				</ul>
				
				<?php 
				$mon=array('Jul','Aug','Sep','Oct','Nov','Dec','Jan','Feb','Mar','Apr','May','Jun');
				$now = new DateTime();
				$month = ((int)$now->format("m"));
				if($month<=7){
					$month=$month-6;
				}
				//$month=5;
				$mon=array_slice($mon,0,$month);
			//	print_r($mon);
				foreach($arr3 as $key=>$val){
				$mon_query=mysqli_query($con,"select month from sch_tran  where Gr_num='".$val[0]."' And fee_type='".$val[1]."' AND sch_tran.Month  in  (".$selectedMonths.") ");
				echo " select month from sch_tran  where Gr_num='".$val[0]."' And fee_type='".$val[1]."' AND sch_tran.Month in  (".$selectedMonths.") ";
				$add=array();
				while($mon_add=mysqli_fetch_array($mon_query)){
					$add[$j]=$mon_add[0];
					$j++;
				}
		?>
			<ul class="table-view">
					<li style="width:200px;">
						<?php echo $val[2];?>
					</li>
					<li style="width:100px;">
						<?php echo $val[4];?>
					</li>
					<li style="width:100px;">
						<?php echo $val[5];?>
					</li>
					<li style="width:100px;">
						<?php echo $val[6];?>
					</li>
					<li style="width:100px;">
						<?php echo $val[0];?>
					</li>
					<li style="width:100px;">
						<?php echo $val[3];
							$monthly=$val[3]+$monthly;
						;?>
					</li>
					<li  class="revenue-pend" style="width:auto;">
						<?php
						//echo "Test";//$add; 
						foreach(array_diff($mon,$add) as $key){
							echo $key;
						}
						?>
					</li>
				</ul>
				<?php
		}
		$x=mysqli_query($con,"select distinct(.sch_tran.Gr_num),Amount from sch_tran 
			 INNER JOIN user_sch ON user_sch.Gr_num = sch_tran.Gr_num
				AND user_sch.Medium = '".$drpMedium."' AND user_sch.Std = '".$drpStd."' 
				AND user_sch.Section = '".$drpSec."' 
				where  sch_tran.Month in  (".$selectedMonths.") 
				order by user_sch.Gr_num ");//sch_tran.month NOT In ('One time') AND 
		$i=0;
		while($y=mysqli_fetch_row($x)){
			$j=0;
			//echo('here');
			//var_dump($y[0]);
			$monthsCharge = 0 ;
			$a=mysqli_query($con,"select Month,Amount from sch_tran where Gr_num='".$y[0]."' AND sch_tran.Month in  (".$selectedMonths.") ");//AND Month Not In('One time')
			while($b=mysqli_fetch_row($a)){
				$mon_fee1[$i][$j++] = $b[0]." " ;	
				$monthsCharge += $b[1];	
			}
			//var_dump($mon_fee1[$i]);
			//$mon_fees = $mon_fee1[$i];//array_diff($mon,$mon_fee1[$i]);
			if(!empty($mon_fee1[$i])){
				?>
				<ul class="table-view">
					<li style="width:200px;">
						<?php foreach(mysqli_fetch_row(mysqli_query($con,"select name from user_sch where Gr_num='".$y[0]."'")) 
						as $p=>$q) echo $q;?>
					</li>

					<li style="width:100px;">
						<?php foreach(mysqli_fetch_row(mysqli_query($con,"select Medium from user_sch where Gr_num='".$y[0]."'")) 
						as $p=>$q) echo $q;?>
					</li>
					<li style="width:100px;">
						<?php foreach(mysqli_fetch_row(mysqli_query($con,"select Std from user_sch where Gr_num='".$y[0]."'")) 
						as $p=>$q) echo $q;?>
					</li>
					<li style="width:100px;">
						<?php foreach(mysqli_fetch_row(mysqli_query($con,"select Section from user_sch where Gr_num='".$y[0]."'")) 
						as $p=>$q) echo $q;?>
					</li>
					<li style="width:100px;">
						<?php echo $y[0];?>
					</li>
					<li style="width:100px;">
						<?php echo $monthsCharge;//$y[1];
							$monthly=$monthly  + $monthsCharge ; //+$y[1];
						;?>
					</li>
					<li  class="revenue-pend" style="width:auto;">
						<?php 
						foreach( $mon_fee1[$i] as $m=>$n)
						print_r( $n);
						//print_r( $mon_fees);
						?>
					</li>
				</ul>
<?php				
}
$i++;
		}

		
		
?>
<ul class="table-view">
					<li class="table-view-header" style="width:100px;">
						
					</li>
					
					<li class="table-view-header" style="width:100px;">
						Total Pending Fee
					</li>
					<li class="table-view-header" style="width:110px;">
						<?php echo $monthly;?>
					</li>
					<li class="table-view-header" style="width:100px;">
						
					</li>
				</ul>
</div>
</div>
</div>
<?php
exit;
?>
